using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class BookingTimeRange : BaseEntites
    {
        // Properties
        public Guid BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; } = null!;
    }
}
