using BookingService.Domain.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingDTO
    {
        [JsonIgnore]
        public Guid Id = Guid.NewGuid();
        public Guid PackageId { get; set; }
        public string PickUpPoint { get; set; } = string.Empty;
        public double DurationWhenBought { get; set; } // The total hours available when the driver bought the package.
        public decimal PriceAtBuyingTime { get; set; } // The price set when they bought the package.

        public Guid InstructorId { get; set; } // Call User microservice to get data.
        public Guid DriverId { get; set; } // Call User microservice to get data.
    }
   

}
