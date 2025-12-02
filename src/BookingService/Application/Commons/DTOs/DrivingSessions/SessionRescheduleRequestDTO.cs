using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionRescheduleRequestDTO
    {
        public string? UserNote { get; set; }
        public DateTime NewStartTime { get; set; }
        public DateTime NewEndTime { get; set; }

        [JsonIgnore]
        public string? JwtToken { get; set; }
    }
}
