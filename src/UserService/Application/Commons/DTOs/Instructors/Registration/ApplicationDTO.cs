using System.Text.Json.Serialization;

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
        public DateTime SumbitDate { get; set; }
        public string Avatar {  get; set; }
        public string DrivingLicenseFront {  get; set; }
        public string DrivingLicenseBack { get; set; }
        public string TeachingLicenseFront { get; set; }
        public string TeachingLicenseBack { get; set; }
        public string HealthCheckup { get; set; }
        public string PersonalProfile { get; set; }
        public IEnumerable<ApplicationTrackingDTO>? TrackingHistories { get; set; }
    }
}
