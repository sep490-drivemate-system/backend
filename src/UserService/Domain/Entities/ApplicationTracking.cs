using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class ApplicationTracking : BaseEntites
    {
        // Property
        public string Note { get; set; }
        public DateTime UpdateAt { get; set; }  
        public bool IsDelete { get; set; }
        public ApplicationStatus Status { get; set; }

        // Key for relationship
        public Guid ApplicationId { get; set; }
        public Guid TypeId { get; set; }

        // Relationship
        public virtual InstructorApplication? InstructorApplication { get; set; }
        public virtual DocumentType? DocumentType { get; set; }

    }
}
