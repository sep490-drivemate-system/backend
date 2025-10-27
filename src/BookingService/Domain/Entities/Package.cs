using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Package: BaseEntites
    {
        // Properties

        public decimal Price { get; set; }
        public TypeRental TypeRental { get; set; }

        // Key for relationship
        public Guid PackageTypeId { get; set; }
        public Guid InstructorId { get; set; }
        public Guid CarId {  get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational Properties
        public  virtual PackageType PackageType { get; set; } 
        public List<Booking>? Bookings{ get; set; }
    }
}
