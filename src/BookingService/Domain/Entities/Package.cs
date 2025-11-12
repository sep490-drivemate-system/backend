using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Package: BaseEntites
    {
        // Properties
        public string Name { get; set; } // This is the package name
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public double Duration { get; set; } // Duration calculated in hours
        public decimal Price { get; set; }
        public bool AllowNoviceVehicle {  get; set; }

        //public PackageRentalType RentalType { get; set; } // No longer required by the requirement

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Keys for relationship
        public Guid InstructorId { get; set; } // Requires call from User microservice to get information.

        // Navigational Properties
        public virtual ICollection<RoadType>? RoadTypes { get; set; }
        public virtual ICollection<DrivingSkill>? DrivingSkills { get; set; }
        public virtual ICollection<Car>? Cars { get; set; }
        public virtual ICollection<Booking>? Bookings{ get; set; }
    }
}
