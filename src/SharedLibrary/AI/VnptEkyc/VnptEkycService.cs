using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibrary.AI.VnptEkyc
{
    internal sealed class VnptEkycService : IVnptEkycService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<VnptEkycOptions> _optionsMonitor;

        public VnptEkycService(HttpClient httpClient, IOptionsMonitor<VnptEkycOptions> optionsMonitor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        }

        private VnptEkycOptions Options => _optionsMonitor.CurrentValue;

        public Task<VnptEkycResponse> AnalyzeDocumentAsync(
            VnptDocumentType documentType,
            Stream frontImage,
            string frontFileName,
            Stream? backImage = null,
            string? backFileName = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(frontImage);

            if (string.IsNullOrWhiteSpace(frontFileName))
            {
                throw new ArgumentException("Front image file name is required.", nameof(frontFileName));
            }

            var options = Options;
            EnsureEndpointConfigured(options.DocumentEndpoint, "document");

            var documentTypeValue = ResolveDocumentType(options, documentType);
            var form = new MultipartFormDataContent();

            form.Add(CreateFileContent(frontImage, frontFileName), options.DocumentFrontFieldName, frontFileName);

            if (backImage != null && !string.IsNullOrWhiteSpace(backFileName))
            {
                form.Add(CreateFileContent(backImage, backFileName), options.DocumentBackFieldName, backFileName);
            }

            if (!string.IsNullOrWhiteSpace(documentTypeValue))
            {
                form.Add(new StringContent(documentTypeValue), options.DocumentTypeFieldName);
            }

            return SendMultipartAsync(options.DocumentEndpoint, form, cancellationToken);
        }

        public Task<VnptEkycResponse> MatchFacesAsync(
            Stream documentPortraitImage,
            string documentPortraitFileName,
            Stream selfieImage,
            string selfieFileName,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(documentPortraitImage);
            ArgumentNullException.ThrowIfNull(selfieImage);

            if (string.IsNullOrWhiteSpace(documentPortraitFileName))
            {
                throw new ArgumentException("Document portrait file name is required.", nameof(documentPortraitFileName));
            }

            if (string.IsNullOrWhiteSpace(selfieFileName))
            {
                throw new ArgumentException("Selfie file name is required.", nameof(selfieFileName));
            }

            var options = Options;
            EnsureEndpointConfigured(options.FaceMatchEndpoint, "face match");

            var form = new MultipartFormDataContent();
            form.Add(CreateFileContent(documentPortraitImage, documentPortraitFileName), options.FaceDocumentFieldName, documentPortraitFileName);
            form.Add(CreateFileContent(selfieImage, selfieFileName), options.FaceSelfieFieldName, selfieFileName);

            return SendMultipartAsync(options.FaceMatchEndpoint, form, cancellationToken);
        }

        public Task<VnptEkycResponse> VerifyLivenessAsync(
            Stream videoStream,
            string videoFileName,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(videoStream);

            if (string.IsNullOrWhiteSpace(videoFileName))
            {
                throw new ArgumentException("Video file name is required.", nameof(videoFileName));
            }

            var options = Options;
            EnsureEndpointConfigured(options.LivenessEndpoint, "liveness");

            var form = new MultipartFormDataContent();
            form.Add(CreateFileContent(videoStream, videoFileName), options.LivenessFieldName, videoFileName);

            return SendMultipartAsync(options.LivenessEndpoint, form, cancellationToken);
        }

        private async Task<VnptEkycResponse> SendMultipartAsync(
            string endpoint,
            MultipartFormDataContent content,
            CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            ApplyCredentials(request.Headers);

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var raw = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"VNPT eKYC responded with {(int)response.StatusCode}: {raw}");
            }

            return ParseResponse(raw);
        }

        private void ApplyCredentials(HttpRequestHeaders headers)
        {
            var options = Options;

            if (string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.ApiSecret))
            {
                throw new InvalidOperationException("VNPT eKYC credentials are not configured.");
            }

            var keyHeader = string.IsNullOrWhiteSpace(options.ApiKeyHeaderName) ? "x-api-key" : options.ApiKeyHeaderName;
            var secretHeader = string.IsNullOrWhiteSpace(options.ApiSecretHeaderName) ? "x-api-secret" : options.ApiSecretHeaderName;

            headers.Remove(keyHeader);
            headers.Remove(secretHeader);
            headers.TryAddWithoutValidation(keyHeader, options.ApiKey);
            headers.TryAddWithoutValidation(secretHeader, options.ApiSecret);
        }

        private static VnptEkycResponse ParseResponse(string responseContent)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return new VnptEkycResponse
                {
                    Code = -1,
                    Message = "Empty response from VNPT eKYC service.",
                    RawResponse = string.Empty
                };
            }

            using var document = JsonDocument.Parse(responseContent);
            var root = document.RootElement;

            var code = ReadInt(root, "code", "error_code");
            var message = ReadString(root, "message", "error_message");
            var requestId = ReadString(root, "request_id", "requestId");
            JsonNode? dataNode = null;

            if (TryGetProperty(root, out var dataElement, "data", "result"))
            {
                dataNode = JsonNode.Parse(dataElement.GetRawText());
            }

            return new VnptEkycResponse
            {
                Code = code,
                Message = message,
                RequestId = requestId,
                Data = dataNode,
                RawResponse = responseContent
            };
        }

        private static int ReadInt(JsonElement root, params string[] propertyNames)
        {
            foreach (var property in propertyNames)
            {
                if (!string.IsNullOrWhiteSpace(property) && root.TryGetProperty(property, out var element))
                {
                    if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var number))
                    {
                        return number;
                    }

                    if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out var parsed))
                    {
                        return parsed;
                    }
                }
            }

            return -1;
        }

        private static string? ReadString(JsonElement root, params string[] propertyNames)
        {
            foreach (var property in propertyNames)
            {
                if (!string.IsNullOrWhiteSpace(property) && root.TryGetProperty(property, out var element))
                {
                    if (element.ValueKind == JsonValueKind.String)
                    {
                        return element.GetString();
                    }
                }
            }

            return null;
        }

        private static bool TryGetProperty(JsonElement root, out JsonElement element, params string[] propertyNames)
        {
            foreach (var property in propertyNames)
            {
                if (!string.IsNullOrWhiteSpace(property) && root.TryGetProperty(property, out element))
                {
                    return true;
                }
            }

            element = default;
            return false;
        }

        private static StreamContent CreateFileContent(Stream stream, string fileName, string? contentType = null)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            var content = new StreamContent(stream);
            var resolvedContentType = contentType ?? ResolveContentType(fileName);
            content.Headers.ContentType = new MediaTypeHeaderValue(resolvedContentType);
            return content;
        }

        private static string ResolveDocumentType(VnptEkycOptions options, VnptDocumentType documentType)
        {
            var key = documentType.ToString();
            if (options.DocumentTypeMapping.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return key.ToLowerInvariant();
        }

        private static void EnsureEndpointConfigured(string? endpoint, string actionName)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new InvalidOperationException($"VNPT eKYC {actionName} endpoint is not configured.");
            }
        }

        private static string ResolveContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".mp4" => "video/mp4",
                ".mov" => "video/quicktime",
                ".heic" => "image/heic",
                ".webp" => "image/webp",
                ".jpeg" or ".jpg" => "image/jpeg",
                _ => "application/octet-stream"
            };
        }
    }
}

