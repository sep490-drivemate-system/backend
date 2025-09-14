using BookingService.Domain.Enum;

namespace BookingService.Domain.Entities
{
    public class DrivingSession
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public SessionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; } = null!;
        public virtual ICollection<SessionRoute> SessionRoutes { get; set; } = new List<SessionRoute>();
    }
}
