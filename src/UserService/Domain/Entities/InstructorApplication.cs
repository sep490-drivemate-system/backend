using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class InstructorApplication : BaseEntites
    {
        // Property
        public string Note { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }
        public DateTime SubmitAt{ get; set; }
        public DateTime ReviewAt { get; set; }
        public ApplicationStatus Status { get; set; }

        // Key for relationship
        public Guid ReviewerId { get; set; }

        // Relationship
        public virtual Instructor? Instructor { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<InstructorDocument> InstructorDocuments { get; set; } = new List<InstructorDocument>();

    }
}
