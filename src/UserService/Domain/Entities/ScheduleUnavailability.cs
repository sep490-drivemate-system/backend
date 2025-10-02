using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class ScheduleUnavailability : BaseEntites
    {
        // Poperties
        public DateOnly Date{ get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Key for relationship
        public Guid InstructorId { get; set; }

        // Relationship
        public virtual Instructor? Instructor { get; set; }

    }
}
