using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Instructors.Registration
{
    public class ApplicationTrackingDTO
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }

    public class ApplicationDTO
    {
        public Guid ApplicationId { get; set; }
        public Guid InstructorId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateOnly DateUntilAutoRejection { get; set; }
        public string Avatar {  get; set; }
        public string DrivingLicenseFront {  get; set; }
        public string DrivingLicenseBack { get; set; }
        public string TeachingLicenseFront { get; set; }
        public DrivingLicenseTier TeachingLicenseTier { get; set; }
        public DrivingLicenseTier DrivingLicenseTier { get; set; }
        public string HealthCheckup { get; set; }
        public string PersonalProfile { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public IEnumerable<ApplicationTrackingDTO>? TrackingHistories { get; set; }
    }
}
