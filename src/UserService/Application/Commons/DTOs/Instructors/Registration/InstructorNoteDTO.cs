using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors.Registration
{
    public class InstructorNoteDTO
    {
        [JsonPropertyName("note")]
        public string Note { get; set; }
    }
}
