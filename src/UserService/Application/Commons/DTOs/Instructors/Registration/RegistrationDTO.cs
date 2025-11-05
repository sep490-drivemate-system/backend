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

        [FromForm(Name = "location")]
        public string? Location { get; set; }

        [FromForm(Name = "birth_date")]
        public DateOnly? BirthDate { get; set; }

        [FromForm(Name = "gender")]
        public string? Gender { get; set; }

        [FromForm(Name = "avatar_image")]
        public IFormFile? Avatar { get; set; }

        [FromForm(Name = "health_checkup")]
        public IFormFile? HealthCheckup { get; set; }

        [FromForm(Name = "personal_profile")]
        public IFormFile? PersonalProfile { get; set; }

        [FromForm(Name = "driving_license_front_image")]
        public IFormFile? DrivingLicenseFront { get; set; }

        [FromForm(Name = "driving_license_back_image")]
        public IFormFile? DrivingLicenseBack { get; set; }

        [FromForm(Name = "teaching_license_front_image")]
        public IFormFile? TeachingLicenseFront { get; set; }

        [FromForm(Name = "teaching_license_back_image")]
        public IFormFile? TeachingLicenseBack { get; set; }

        [FromForm(Name = "allowed_teaching_tier")]
        public string? TeachingTier { get; set; }
    }
}
