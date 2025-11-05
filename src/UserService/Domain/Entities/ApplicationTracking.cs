using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class ApplicationTracking : BaseEntites
    {
        // Properties
        public string Note { get; set; }
        public ApplicationStatus Status { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Key for relationship
        public Guid ApplicationId { get; set; }

        // Relationship
        public virtual InstructorApplication? InstructorApplication { get; set; }
    }
}
