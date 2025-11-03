using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorScheduleDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }
    }
}
