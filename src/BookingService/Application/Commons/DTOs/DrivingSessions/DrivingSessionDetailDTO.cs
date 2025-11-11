using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionDetailDTO
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public string PackageName { get; set; }
        public string NoviceDriverName { get; set; }
        public string DisplayName{ get; set; }
        public string? NoviceAvatar { get; set; }
        public string Date { get; set; } = string.Empty;              // "2025-11-12"
        public string StartTime { get; set; } = string.Empty;         // "09:00"
        public string EndTime { get; set; } = string.Empty;           // "12:00"
        public double Duration { get; set; }                          // hours
        public string Location { get; set; } = string.Empty;          
        public Guid? VehicleId { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }
        public string? VehicleName { get; set; }                      // "Toyota Vios 2023"
        public SessionStatus Status { get; set; }
        public string StatusDisplayString { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool HasRoute { get; set; }
        public decimal? PriceForCar { get; set; }
    }
}
