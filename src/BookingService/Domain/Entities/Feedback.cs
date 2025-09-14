namespace BookingService.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; } = null!;
    }
}
