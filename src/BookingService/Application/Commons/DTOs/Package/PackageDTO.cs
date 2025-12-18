namespace BookingService.Application.Commons.DTOs.Package
{
    public class PackageDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorAvatar { get; set; }
        public bool IsRentalCar { get; set; }
        public double Duration { get; set; } // Duration calculated in hours
        public IEnumerable<string> RoadTypes { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public decimal Price { get; set; }
        public int CarCount { get; set; }
        public int BookingCount { get; set; }
    }
}
