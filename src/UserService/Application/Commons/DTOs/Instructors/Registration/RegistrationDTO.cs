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
        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }
        #endregion

        #region Driving License
        public IFormFile? DrivingLicenseFront { get; set; }
        public IFormFile? DrivingLicenseBack { get; set; }
        public string? DrivingLicenseTier { get; set; }
        #endregion

        #region Teaching License
        public IFormFile? TeachingLicenseFront { get; set; }
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
