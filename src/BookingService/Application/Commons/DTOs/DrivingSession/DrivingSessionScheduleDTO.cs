using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSession
{
    public class DrivingSessionScheduleDTO
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SessionStatus Status { get; set; }
    }
}
