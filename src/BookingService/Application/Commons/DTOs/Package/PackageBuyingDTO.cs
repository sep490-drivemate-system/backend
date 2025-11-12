using BookingService.Domain.Enum;

namespace BookingService.Application.Commons.DTOs.Package
{
    public class PackageBuyingDTO
    {
        public double DurationWhenBought { get; set; } 
        public decimal PriceAtBuyingTime { get; set; } 
        public Guid? CarId { get; set; }
        public Guid PackageId { get; set; }
        public Guid InstructorId { get; set; } 
        public Guid DriverId { get; set; } 
       
    }
}
