using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors.Registration
{
    public class ApplicationTrackingDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("inspector_note")]
        public string Note { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class ApplicationDTO
    {
        [JsonPropertyName("id")]
        public Guid ApplicationId { get; set; }

        [JsonPropertyName("instructor_id")]
        public Guid InstructorId { get; set; }

        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("birthdate")]
        public DateOnly BirthDate { get; set; }

        [JsonPropertyName("submit_date")]
        public DateTime SumbitDate { get; set; }

        [JsonPropertyName("avatar_image_url")]
        public string Avatar {  get; set; }

        [JsonPropertyName("driving_license_front_url")]
        public string DrivingLicenseFront {  get; set; }

        [JsonPropertyName("driving_license_back_url")]
        public string DrivingLicenseBack { get; set; }

        [JsonPropertyName("teaching_license_front_url")]
        public string TeachingLicenseFront { get; set; }

        [JsonPropertyName("teaching_license_back_url")]
        public string TeachingLicenseBack { get; set; }

        [JsonPropertyName("health_checkup_image_url")]
        public string HealthCheckup { get; set; }

        [JsonPropertyName("personal_profile_image_url")]
        public string PersonalProfile { get; set; }

        [JsonPropertyName("tracking_history")]
        public IEnumerable<ApplicationTrackingDTO>? TrackingHistories { get; set; }
    }
}
