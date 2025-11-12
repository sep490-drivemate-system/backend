namespace BookingService.Application.Commons.DTOs.Package
{
    public class PackageDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InstructorName { get; set; }
        public string InstructorAvatar { get; set; }
        public bool HasVehicle { get; set; }
        public int Duration { get; set; }
        public List<string> RoadTypes { get; set; }
        public List<string> Skills { get; set; }
        public decimal Price { get; set; }
        public int? BookingCount { get; set; }
    }
}
