using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionListDTO
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public string DisplayStartLocationName { get; set; } = string.Empty;
        public string DisplayEndLocationName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;             
        public string StartTime { get; set; } = string.Empty;        
        public string EndTime { get; set; } = string.Empty;           
        public double Duration { get; set; }                         
        public string Location { get; set; } = string.Empty;          
        public Guid? VehicleId { get; set; }
        public string? VehicleName { get; set; }                     
        public SessionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
