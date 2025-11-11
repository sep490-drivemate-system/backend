using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSession
{
    public class DrivingSessionScheduleDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
