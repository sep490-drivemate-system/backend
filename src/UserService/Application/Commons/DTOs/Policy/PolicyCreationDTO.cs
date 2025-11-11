using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Policy
{
    public class PolicyCreationDTO
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
