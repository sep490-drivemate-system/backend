using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Package: BaseEntites
    {
        // Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public double Duration { get; set; }
        public decimal Price { get; set; }
        public bool IsRentalCar {  get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Keys for relationship
        public Guid InstructorId { get; set; } 

        // Navigational Properties
        public virtual ICollection<RoadType>? RoadTypes { get; set; }
        public virtual ICollection<DrivingSkill>? DrivingSkills { get; set; }
        public virtual ICollection<Car>? Cars { get; set; }
        public virtual ICollection<Booking>? Bookings{ get; set; }
    }
}
