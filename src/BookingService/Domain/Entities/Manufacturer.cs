using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Manufacturer : BaseEntites
    {
        // Properties
        public string Name { get; set; }

        // System properties
        public DateTime LastModifiedAt {  get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual ICollection<Car>? Cars { get; set; } 
    }
}
