using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class DrivingSession: BaseEntites
    {
        // Properties
        public Guid BookingId { get; set; }

        public DateTime Date { get; set; }
        
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public DateTime ActualStart { get; set; }

        public DateTime ActualEnd { get; set; }

        public decimal TotalDistance { get; set; }

        public decimal AverageSpeed { get; set; }

        public decimal StartingLatitude { get; set; }

        public decimal StartingLongtitude { get; set; }

        public decimal EndingLatitude { get; set; }

        public decimal EndingLongtitude { get; set; }

        public SessionStatus Status { get; set; }

        // System property

        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public Booking Bookings { get; set; }

        public List<SessionRoadType> SessionRoadTypes { get; set; }

        public List<SessionLog> SessionLogs { get; set; }

        public List<SessionRoute> SessionRoutes { get; set; }
    }
}
