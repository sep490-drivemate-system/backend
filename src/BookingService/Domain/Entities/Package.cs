using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Package: BaseEntites
    {
        // Properties
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal RecommendedValue { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational Properties
        public List<CarPackage> CarPackages { get; set; }
    }
}
