using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    /// <summary>
    ///     The package bought by the novice driver.
    /// </summary>
    public class Booking: BaseEntites
    {
        // Properties
        public double DurationWhenBought { get; set; } // The total hours available when the driver bought the package.
        public decimal PriceAtBuyingTime { get; set; } // The price set when they bought the package.
        public BookingStatus Status { get; set; }

        // Keys for relationships
        public Guid CarId {  get; set; }
        public Guid PackageId { get; set; }
        public Guid InstructorId { get; set; } // Call User microservice to get data.
        public Guid DriverId { get; set; } // Call User microservice to get data.

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual Package? Package { get; set; }
        public virtual Car? Car { get; set; }
        public virtual Feedback? Feedback { get; set; }
        public virtual ICollection<DrivingSession>? DrivingSessions { get; set; }

    }
}
