using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class DrivingSession : BaseEntites
    {
        // Properties
        public Guid BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public SessionStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; } = null!;
        public virtual ICollection<SessionRoute> SessionRoutes { get; set; } = new List<SessionRoute>();
    }
}
