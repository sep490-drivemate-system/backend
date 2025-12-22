using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace SharedLibrary.SharedKernel.Http.DTOs.User
{
    public class InstructorDetailDTO
    {
        public Guid InstructorId { get; set; }
        public string Bio { get; set; }
        public int ExperienceYear { get; set; }

    }

    public class NoviceDriverDetailDTO
    {
        public Guid NoviceDriverId { get; set; }
        public string DrivingLicense { get; set; }
        public DateOnly DrivingLicenseExpirationDate { get; set; }


    }

    public class UserDetailDTO
    {
        public Guid UserId { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
        public int AccountStatus { get; set; }
        public DrivingLicenseTier LicenseTier { get; set; }
        public DateOnly BirthDate { get; set; }
        public UserRole Role { get; set; }
        public InstructorDetailDTO? Instructor {  get; set; }
        public NoviceDriverDetailDTO? NoviceDriver { get; set; }
    }
}
