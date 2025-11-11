using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors.Feedback
{
    public class InstructorFeedback
    {
        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}
