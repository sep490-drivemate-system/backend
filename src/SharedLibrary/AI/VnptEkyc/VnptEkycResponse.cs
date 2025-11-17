using System.Text.Json;
using System.Text.Json.Nodes;

namespace SharedLibrary.AI.VnptEkyc
{
    public sealed class VnptEkycResponse
    {
        public int Code { get; init; }
        public string? Message { get; init; }
        public string? RequestId { get; init; }
        public JsonNode? Data { get; init; }
        public string RawResponse { get; init; } = string.Empty;

        public bool IsSuccess => Code == 0;

        public T? DeserializeData<T>(JsonSerializerOptions? options = null)
        {
            return Data is null ? default : Data.Deserialize<T>(options ?? _defaultOptions);
        }

        private static readonly JsonSerializerOptions _defaultOptions = new(JsonSerializerDefaults.Web);
    }
}

