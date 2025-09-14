namespace BookingService.Domain.Entities
{
    public class RouteLog
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public int EstimatedDuration { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<SessionRoute> SessionRoutes { get; set; } = new List<SessionRoute>();
    }
}
