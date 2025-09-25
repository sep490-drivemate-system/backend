using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Package : BaseEntites
    {
        //Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInHours { get; set; }
        public int SessionCount { get; set; }
        public bool IsActive { get; set; }        
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<PackageInstructor> PackageInstructors { get; set; } = new List<PackageInstructor>();
    }
}
