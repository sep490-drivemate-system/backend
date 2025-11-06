using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("fullname")]
        public string FullName { get; set; }
        [JsonPropertyName("avatar_url")]
        public string Avatar { get; set; }
        [JsonPropertyName("bio")]
        public string Bio { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
        [JsonPropertyName("birthdate")]
        public DateOnly Birthdate { get; set; }
        [JsonPropertyName("experiences_year")]
        public int ExperienceYear { get; set; }
        [JsonPropertyName("booking_count")]
        public int BookingCount { get; set; }
        [JsonPropertyName("average_rating")]
        public decimal AverageRating { get; set; }
    }
    public class InstructorDetailDTO : InstructorDTO
    {
    }

}
