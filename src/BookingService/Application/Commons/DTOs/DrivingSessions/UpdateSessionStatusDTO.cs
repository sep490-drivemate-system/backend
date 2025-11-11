using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class UpdateSessionStatusDTO
    {
        public SessionStatus Status { get; set; }
        public string? Note { get; set; }
    }
}
