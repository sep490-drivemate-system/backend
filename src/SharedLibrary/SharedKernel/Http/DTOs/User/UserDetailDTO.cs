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
        public Guid UserId { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
        public DrivingLicenseTier LicenseTier { get; set; }
        public DateOnly BirthDate { get; set; }
        public UserRole Role { get; set; }
        public InstructorDetailDTO? Instructor {  get; set; }
        public NoviceDriverDetailDTO? NoviceDriver { get; set; }
    }
}
