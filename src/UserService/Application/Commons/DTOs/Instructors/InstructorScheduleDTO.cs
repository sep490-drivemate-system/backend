using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorScheduleDTO
    {
        public DateOnly StartTime { get; set; }
        public DateOnly EndTime { get; set; }
    }
}
