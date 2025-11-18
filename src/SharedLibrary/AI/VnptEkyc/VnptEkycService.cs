using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibrary.AI.VnptEkyc
{
    internal sealed class VnptEkycService : IVnptEkycService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public VnptEkycService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ;
            _configuration = configuration;
        }

      
        public Task<VnptEkycResponse> VerifyCitizenIdentity(
            Stream frontImage,
            Stream backImage)
        {
            ArgumentNullException.ThrowIfNull(frontImage);
            ArgumentNullException.ThrowIfNull(backImage);




            return VerifyCitizenIdentityInternal(frontImage, backImage);
        }

       

        private async Task<VnptEkycResponse> VerifyCitizenIdentityInternal(
            Stream frontImage,
            Stream backImage)
        {
            var frontPayload = await ConvertStreamToBase64Async(frontImage).ConfigureAwait(false);
            var backPayload = await ConvertStreamToBase64Async(backImage).ConfigureAwait(false);

            var body = new
            {
                img_front = frontPayload,
                img_back = backPayload,
                client_session = _configuration["VNPT:CLIENT_SESSION"],
                type = _configuration["VNPT:TYPE"],
                crop_param = _configuration["VNPT:CROP_PARAM"],
                validate_postcode = _configuration["VNPT:VALIDATE_POSTCODE"],
                token = _configuration["VNPT:TOKEN"],
            };

            return await SendJsonAsync(body).ConfigureAwait(false);
        }

        private async Task<VnptEkycResponse> SendJsonAsync<TPayload>(
            TPayload payload)
        {
            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            using var request = new HttpRequestMessage(HttpMethod.Post, _configuration["VNPT:CIENDPOINT"])
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            ApplyCitizenHeaders(request.Headers);

            using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var raw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"VNPT eKYC responded with {(int)response.StatusCode}: {raw}");
            }

            return ParseResponse(raw);
        }

       

        private void ApplyCitizenHeaders(HttpRequestHeaders headers)
        {

            headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["VNPT:ACCESS_TOKEN"]);

            headers.Remove("Token-id");
            headers.TryAddWithoutValidation("Token-id", _configuration["VNPT:TOKEN_ID"]);

            headers.Remove("Token-key");
            headers.TryAddWithoutValidation("Token-key", _configuration["VNPT:TOKEN_KEY"]);

            headers.Remove("mac-address");
            headers.TryAddWithoutValidation("mac-address", _configuration["VNPT:MAC_ADDRESS"]);
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
       

        private static async Task<string> ConvertStreamToBase64Async(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            return Convert.ToBase64String(memoryStream.ToArray());
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

