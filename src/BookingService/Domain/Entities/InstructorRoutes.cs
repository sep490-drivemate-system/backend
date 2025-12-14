
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class InstructorRoutes : BaseEntites
    {
        // Properties
        public string RouteName { get; set; }
        public string Polyline { get; set; }
        public decimal StartingLatitude { get; set; }
        public decimal StartingLongtitude { get; set; }
        public string DisplayEndLocationName { get; set; }
        public string DisplayStartLocationName { get; set; }
        public decimal EndingLatitude { get; set; }
        public decimal EndingLongtitude { get; set; }

        // Keys for relationships
        public Guid InstructorId { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Relationship navigation
        public virtual ICollection<Package>? Packages{ get; set; }
    }
}
