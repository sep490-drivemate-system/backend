using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SharedLibrary.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class InstructorApplication : BaseEntites
    {
        // Citizen ID
        public string CitizenIdFront { get; set; } 
        public string CitizenIdBack { get; set; } 
        public string CitizenIdNumber { get; set; }
        public DateOnly CitizenIssueDate { get; set; }
        public DateTime CitizenExpiryDate { get; set; }
        public string CitizenIssuePlace { get; set; }
        public string PermanentAddress { get; set; }
        public VerificationStatus CitizenIdStatus { get; set; }

        // Driving License
        public string DrivingLicenseFront { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public string DrivingLicenseBack { get; set; }      
        public DateOnly? DrivingLicenseExpiry { get; set; }
        public DateOnly DrivingLicenseIssueDate { get; set; }

        public VerificationStatus DrivingLicenseStatus { get; set; }

        // Teaching License
        public string TeachingLicenseFront { get; set; } 
        public string TeachingLicenseBack { get; set; }
        public VerificationStatus TeachingStatus { get; set; }

        // Background Profile
        public string BackgroundProfile { get; set; }
        public VerificationStatus BackgroundProfileStatus { get; set; }

        public DateTime SubmitAt { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Key for relationship


        // Relationship
        public virtual Instructor? Instructor{ get; set; }
        public virtual ICollection<ApplicationTracking>? ApplicationTracking { get; set; }
    }
}
