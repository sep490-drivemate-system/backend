namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionRouteCreateDTO
    {
        public string PolylineSesionRoute { get; set; }
        public List<SessionRoutesDTO> Routes { get; set; }

    }
    public class SessionRoutesDTO 
    {
            public string TextInstruction { get; set; }
            public string StreetName { get; set; }
            public decimal LatitudeStart { get; set; }
            public decimal LongitudeStart { get; set; }
        
    }
}
