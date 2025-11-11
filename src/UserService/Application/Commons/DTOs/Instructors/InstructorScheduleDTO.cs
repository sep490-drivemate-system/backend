using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorScheduleDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
