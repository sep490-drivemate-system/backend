namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class InstructorScheduleResponseDTO
    {
        public DateTime Date { get; set; }
        public List<InstructorScheduleDTO> BusySlots { get; set; } = new List<InstructorScheduleDTO>();
    }
}
