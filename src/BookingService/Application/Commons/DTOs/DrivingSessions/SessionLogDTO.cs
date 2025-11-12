namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionLogDTO
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string StreetName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Heading { get; set; }
        public decimal Speed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
