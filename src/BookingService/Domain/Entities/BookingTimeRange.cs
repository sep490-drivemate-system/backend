namespace BookingService.Domain.Entities
{
    public class BookingTimeRange
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; } = null!;
    }
}
