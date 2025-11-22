using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class DrivingSessionDetailDTO
    {
        public Guid Id { get; set; }
        public string PackageName { get; set; }
        public string DisplayStartLocationName{ get; set; }
        public string Date { get; set; } = string.Empty;              
        public string StartTime { get; set; } = string.Empty;       
        public string EndTime { get; set; } = string.Empty;          
        public double Duration { get; set; }                              
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }
        public string DisplayEndLocationName { get; set; }
        public decimal EndingLatitude { get; set; }
        public decimal EndingLongtitude { get; set; }
        public string? CarName { get; set; }                     
        public SessionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DrivingSessionlDTO
    {
        public string DisplayStartLocationName { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }
        public string DisplayEndLocationName { get; set; }
        public decimal EndingLatitude { get; set; }
        public decimal EndingLongtitude { get; set; }
        public SessionStatus Status { get; set; }
    }

}
