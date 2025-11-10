using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors.Registration
{
    public class RegistrationDTO
    {
        [FromForm(Name = "fullname")]
        public string? Fullname { get; set; }

        [FromForm(Name = "password")]
        public string? RawPassword { get; set; }

        [FromForm(Name = "email")]
        public string? Email { get; set; }

        [FromForm(Name = "phone")]
        public string? PhoneNumber { get; set; }

        [FromForm(Name = "avatar_image")]
        public IFormFile? Avatar { get; set; }

        #region National Id
        [FromForm(Name = "national_id")]
        public string? NationalIdNumber { get; set; }

        [FromForm(Name = "permanent_address")]
        public string? Location { get; set; }

        [FromForm(Name = "national_id_expiration_date")]
        public DateOnly? NationalExpiryDate { get; set; }

        [FromForm(Name = "national_id_issues_date")]
        public DateOnly? NationalIssusesDate { get; set; }

        [FromForm(Name = "national_id_issues_location")]
        public string? IssuedLocation { get; set; }

        [FromForm(Name = "birthdate")]
        public DateOnly? BirthDate { get; set; }

        [FromForm(Name = "gender")]
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
        [FromForm(Name = "teaching_license_front_image")]
        public IFormFile? TeachingLicenseFront { get; set; }

        [FromForm(Name = "teaching_license_back_image")]
        public IFormFile? TeachingLicenseBack { get; set; }

        [FromForm(Name = "allowed_teaching_tier")]
        public string? TeachingTier { get; set; }
        #endregion

        #region Health checkup
        [FromForm(Name = "health_checkup")]
        public IFormFile? HealthCheckup { get; set; }
        #endregion

        #region Personal Records
        [FromForm(Name = "personal_profile")]
        public IFormFile? PersonalProfile { get; set; }
        #endregion
    }
}
