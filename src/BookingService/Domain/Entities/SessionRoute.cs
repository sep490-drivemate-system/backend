using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class SessionRoute : BaseEntites
    {
        // Properties
        public Guid SessionId { get; set; }
        public Guid RouteId { get; set; }     

        // Navigation properties
        public virtual DrivingSession Session { get; set; } = null!;
        public virtual RouteLog RouteLog { get; set; } = null!;
    }
}
