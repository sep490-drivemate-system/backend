using System.Collections.Generic;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionRouteResponseDTO
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string TextInstruction { get; set; }
        public string StreetName { get; set; }
        public decimal LatitudeStart { get; set; }
        public decimal LongitudeStart { get; set; }
    }
}
