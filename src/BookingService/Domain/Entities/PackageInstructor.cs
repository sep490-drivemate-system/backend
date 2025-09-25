using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class PackageInstructor : BaseEntites
    {
        public Guid PackageId { get; set; }
        public Guid InstructorId { get; set; }     

        // Navigation properties
        public virtual Package Package { get; set; } = null!;
    }
}
