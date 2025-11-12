namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionLogCreateDTO
    {
        public string StreetName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Heading { get; set; }
        public decimal Speed { get; set; }
    }
}
