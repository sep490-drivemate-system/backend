using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class DrivingSession: BaseEntites
    {
        // Properties
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

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        
        // Keys for relationships
        public Guid BookingId { get; set; }

        // Navigational properties
        public virtual Booking? Booking { get; set; }
        public virtual ICollection<SessionLog>? SessionLogs { get; set; }
        public virtual ICollection<SessionRoute>? SessionRoutes { get; set; }
        public virtual ICollection<RescheduleRequest>? RescheduleRequests { get; set; }
    }
}
