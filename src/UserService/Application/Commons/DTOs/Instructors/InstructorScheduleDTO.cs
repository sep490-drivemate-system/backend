using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorScheduleDTO
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
