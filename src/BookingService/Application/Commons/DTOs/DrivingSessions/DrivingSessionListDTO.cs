using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionListDTO
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;              // "2025-11-01"
        public string StartTime { get; set; } = string.Empty;         // "08:00"
        public string EndTime { get; set; } = string.Empty;           // "11:00"
        public double Duration { get; set; }                          // hours
        public string Location { get; set; } = string.Empty;          // "123 Nguyễn Huệ, Q1, TP.HCM"
        public Guid? VehicleId { get; set; }
        public string? VehicleName { get; set; }                      // "Toyota Vios 2023"
        public SessionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
