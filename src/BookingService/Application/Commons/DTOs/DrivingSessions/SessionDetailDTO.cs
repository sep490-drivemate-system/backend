using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionDetailDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal? PriceForCar { get; set; }
        public DateTime ActualStart { get; set; }
        public DateTime ActualEnd { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal AverageSpeed { get; set; }
        public string DisplayStartLocationName { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }
        public string DisplayEndLocationName { get; set; }
        public decimal EndingLatitude { get; set; }
        public decimal EndingLongtitude { get; set; }
        public string? NoviceDriverNote { get; set; }
        public string? InstructorNote { get; set; }

        public SessionStatus Status { get; set; }

        public List<RouteDetailDTO>? RouteDetails { get; set; }
        public List<LogDetailDTO>? LogDetails { get; set; }
    }

    public class RouteDetailDTO
    {
        public string TextInstruction { get; set; }

        public string StreetName { get; set; }

        public decimal LatitudeStart { get; set; }

        public decimal LongitudeStart { get; set; }
    }
    public class LogDetailDTO
    {
        public string StreetName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Heading { get; set; }
        public decimal Speed { get; set; }
    }
}
