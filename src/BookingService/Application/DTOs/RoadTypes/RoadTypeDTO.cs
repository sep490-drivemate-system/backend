using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BookingService.Application.DTOs.RoadTypes
{
    public class RoadTypeDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
