using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class InstructorApplication : BaseEntites
    {
        // Properties

        // User personal information
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        // National Id
        public string NationalId { get; set; }
        public string Fullname { get; set; }
        public string PermanentAddress { get; set; }
        public string NationalIdIssuedLocation { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public DateOnly NationalIdIssuedDate { get; set; }
        public DateOnly NationalIdExpiryDatee { get; set; }

        // Driving License
        public string DrivingLicenseFront { get; set; }
        public string DrivingLicenseBack { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public DateOnly DrivingLicenseIssuesDate { get; set; }
        public DateOnly DrivingLicenseExpiryDate {  get; set; }
        public DrivingLicenseTier DrivingLicenseTier { get; set; }

        // Teaching License
        public string TeachingLicenseFront { get; set; }
        public string TeachingLicenseBack { get; set; }
        public DrivingLicenseTier TeachingLicenseTier { get; set; }

        // Health Checkup
        public string HealthCheckup { get; set; }

        // Background Profile
        public string BackgroundProfile { get; set; }

        // Application properties
        public DateTime SubmitAt { get; set; }
        public ApplicationStatus Status { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Keys for relationship
        public Guid InstructorId { get; set; }

        // Navigational properties
        public virtual Instructor? Instructors { get; set; }
        public virtual ICollection<ApplicationTracking>? ApplicationTrackings { get; set; }
    }
}
