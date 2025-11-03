using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class PersonalSchedule : BaseEntites
    {
        // Poperties
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Key for relationship
        public Guid InstructorId { get; set; }

        // Relationship
        public virtual Instructor? Instructor { get; set; }

    }
}
