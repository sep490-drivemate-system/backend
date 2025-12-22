using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;

namespace BookingService.Domain.Entities
{
    public class Car : BaseEntites
    {
        // Properties
        public string LicensePlate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DocumentJsonBlobString { get; set; } // This will be used to replace car documents
        public DrivingLicenseTier LicenseTier { get; set; } // Requires call from User microservice to get data.
        public string CarType { get; set; }
        public string FuelType { get; set; }
        public int SeatCount { get; set; }
        public DateOnly InsuranceEndTime { get; set; }
        public CarStatus Status { get; set; }
        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Keys for relationship
        public Guid InstructorId { get; set; } // Requires call from User microservice to get data.
        public Guid ManufacturerId { get; set; }

        // Navigational properties
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<CarImage>? CarImages { get; set; }
        public virtual ICollection<Package>? Packages { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}
