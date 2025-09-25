using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class RouteLog : BaseEntites
    {
        public string Name { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public int EstimatedDuration { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<SessionRoute> SessionRoutes { get; set; } = new List<SessionRoute>();
    }
}
