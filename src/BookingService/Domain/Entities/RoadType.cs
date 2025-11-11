using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class RoadType: BaseEntites
    {
        // Properties
        public string Name { get; set; }
        public string? Description { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual List<Package> Packages { get; set; }

    }
}
