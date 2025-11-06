using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
