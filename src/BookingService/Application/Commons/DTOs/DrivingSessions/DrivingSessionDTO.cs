using BookingService.Domain.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionDTO
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; } // Used to get the bought package information.

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("status")]
        public SessionStatus Status { get; set; }

        [JsonPropertyName("status_display")]
        public string StatusDisplayString { get; set; }

        [JsonPropertyName("pickup_location_display")]
        public string? PickupLocation { get; set; } // Currently always null!

        [JsonPropertyName("pickup_long")]
        public decimal Longtitude { get; set; }

        [JsonPropertyName("pickup_lat")]
        public decimal Latitude { get; set; }
    }
}
