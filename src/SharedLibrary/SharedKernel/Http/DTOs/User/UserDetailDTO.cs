using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace SharedLibrary.SharedKernel.Http.DTOs.User
{
    public class InstructorDetailDTO
    {
        [JsonPropertyName("instructor_id")]
        public Guid InstructorId { get; set; }
        [JsonPropertyName("bio")]
        public string Bio { get; set; }
        [JsonPropertyName("experiences")]
        public int ExperienceYear { get; set; }
    }

    public class NoviceDriverDetailDTO
    {
        [JsonPropertyName("driver_id")]
        public Guid NoviceDriverId { get; set; }
    }

    public class UserDetailDTO
    {
        [JsonPropertyName("id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }
        [JsonPropertyName("birthdate")]
        public DateOnly BirthDate { get; set; }
        [JsonPropertyName("role")]
        public UserRole Role { get; set; }
        [JsonPropertyName("instructor")]
        public InstructorDetailDTO? Instructor {  get; set; }
        [JsonPropertyName("driver")]
        public NoviceDriverDetailDTO? NoviceDriver { get; set; }
    }
}
