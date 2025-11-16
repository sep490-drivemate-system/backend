using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class DrivingSession: BaseEntites
    {
        // Properties
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

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        
        // Keys for relationships
        public Guid BookingId { get; set; }

        // Navigational properties
        public virtual Booking Booking { get; set; }
        public virtual ICollection<SessionLog>? SessionLogs { get; set; }
        public virtual ICollection<SessionRoute>? SessionRoutes { get; set; }
        public virtual ICollection<RescheduleRequest>? RescheduleRequests { get; set; }
    }
}
