using System.Collections.Generic;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionRouteResponseDTO
    {
        public decimal SessionStartingLat { get; set; }
        public decimal SessionStartingLong { get; set; }
        public List<SessionRouteDTO> Routes { get; set; } = new List<SessionRouteDTO>();
    }
}
