namespace BookingService.Application.Commons.DTOs.InstructorRoutes
{
    public class InstructorRouteDTO
    {
        public Guid? Id { get; set; }
        public string RouteName { get; set; }
        public string Polyline { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }
        public string DisplayEndLocationName { get; set; }
        public string DisplayStartLocationName { get; set; }
        public decimal EndingLatitude { get; set; }
        public decimal EndingLongtitude { get; set; }
        public Guid? InstructorId { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}


