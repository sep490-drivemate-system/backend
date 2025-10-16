using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class DrivingSkill : BaseEntites
    {
        // Properties
        public string Name { get; set; }

        // System property

        public string LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation 
        public virtual ICollection<Booking> Bookings { get; set; } 
    }
}
