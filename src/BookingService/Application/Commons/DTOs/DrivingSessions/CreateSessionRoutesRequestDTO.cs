namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class CreateSessionRoutesRequestDTO
    {
        public Guid SessionId { get; set; }
        public List<SessionRouteCreateDTO> Routes { get; set; } = new();
    }
}
