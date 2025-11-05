using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Policy
{
    public class PolicyDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Detail { get; set; }
    }
}
