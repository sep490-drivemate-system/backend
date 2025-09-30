using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class CarPackage: BaseEntites
    {
        // Properties
        public Guid CarId { get; set; }

        public Guid PackageId { get; set; }

        public decimal Price { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational Properties
        public List<Booking> Bookings { get; set; }

        public Package Packages { get; set; }
    }
}
