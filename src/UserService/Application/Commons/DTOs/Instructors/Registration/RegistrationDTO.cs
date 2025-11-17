using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors.Registration
{
    public class RegistrationDTO
    {
        public string? Fullname { get; set; }
        public string? RawPassword { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Avatar { get; set; }

        #region National Id
        public string? NationalIdNumber { get; set; }
        public string? Location { get; set; }
        public DateOnly? NationalExpiryDate { get; set; }
        public DateOnly? NationalIssusesDate { get; set; }
        public string? IssuedLocation { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }
        #endregion

        #region Driving License
        [FromForm(Name = "driving_license_front_image")]
        public IFormFile? DrivingLicenseFront { get; set; }

        [FromForm(Name = "driving_license_back_image")]
        public IFormFile? DrivingLicenseBack { get; set; }

        [FromForm(Name = "driving_license_number")]
        public string? DrivingLicenseNumber { get; set; }

        [FromForm(Name = "driving_license_issues_date")]
        public DateOnly? DrivingLicenseIssuesDate { get; set; }

        [FromForm(Name = "driving_license_expiry_date")]
        public DateOnly? DrivingLicenseExpiryDate { get; set; }

        [FromForm(Name = "driving_license_tier")]
        public string? DrivingLicenseTier { get; set; }
        #endregion

        #region Teaching License
        public IFormFile? TeachingLicenseFront { get; set; }
        public IFormFile? TeachingLicenseBack { get; set; }
        public string? TeachingTier { get; set; }
        #endregion

        #region Health checkup
        public IFormFile? HealthCheckup { get; set; }
        #endregion

        #region Personal Records
        public IFormFile? PersonalProfile { get; set; }
        #endregion
    }
}
