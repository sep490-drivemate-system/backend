using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionCreationDTO
    {
        [JsonPropertyName("booking_id")] // This should have been bought_package_id
        public Guid BookingId { get; set; }

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("duration")] 
        public int Duration { get; set; } // Duration will be calculated in minutes

        [JsonPropertyName("note")]
        public string? SessionNote { get; set; } // This is optional for novice driver to write.
    }
}
