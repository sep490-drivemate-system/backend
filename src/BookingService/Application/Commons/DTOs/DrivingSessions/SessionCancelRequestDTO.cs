using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionCancelRequestDTO
    {
        [JsonPropertyName("note")]
        public string? UserNote { get; set; }

        [JsonIgnore]
        public string? JwtToken { get; set; }
    }
}
