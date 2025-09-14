namespace BookingService.Domain.Entities
{
    public class SessionRoute
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Guid RouteId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual DrivingSession Session { get; set; } = null!;
        public virtual RouteLog RouteLog { get; set; } = null!;
    }
}
