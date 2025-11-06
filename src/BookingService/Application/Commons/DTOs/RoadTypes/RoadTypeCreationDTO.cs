using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.RoadTypes
{
    public class RoadTypeCreationDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
