using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionCreationDTO
    {
        public Guid BookingId { get; set; }

        public DateTime StartTime { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }

        public int Duration { get; set; } // Duration will be calculated in minutes
        public string? SessionNote { get; set; } // This is optional for novice driver to write.
    }
}
