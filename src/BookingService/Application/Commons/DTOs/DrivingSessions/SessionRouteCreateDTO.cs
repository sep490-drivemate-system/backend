namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionRouteCreateDTO
    {
        public string TextInstruction { get; set; }
        public string StreetName { get; set; }
        public decimal LatitudeStart { get; set; }
        public decimal LongitudeStart { get; set; }
    }
}
