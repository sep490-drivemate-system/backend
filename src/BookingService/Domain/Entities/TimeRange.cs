using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class TimeRange: BaseEntites
    {
        // Properties
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        
        public Guid BookingId { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public Booking Bookings { get; set; }
    }
}
