using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class PackageType : BaseEntites
    {
        // propertity
        public PackageCategory Type { get; set; } 

        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? Value { get; set; }
        // key for navigation 

        // relationship

        public virtual ICollection<Package> Packages { get; set; }
    }
}
