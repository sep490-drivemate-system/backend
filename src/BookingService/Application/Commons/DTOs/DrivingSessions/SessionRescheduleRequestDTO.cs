using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionRescheduleRequestDTO
    {
        [JsonPropertyName("note")]
        public string? UserNote { get; set; }

        [JsonPropertyName("reschedule_start_time")]
        public DateTime NewStartTime { get; set; }

        [JsonPropertyName("reschedule_end_time")]
        public DateTime NewEndTime { get; set; }

        [JsonIgnore]
        public string? JwtToken { get; set; }
    }
}
