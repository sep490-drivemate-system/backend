using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class DocumentType : BaseEntites
    {
        // Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Relationship
        public virtual ICollection<InstructorDocument>? InstructorDocuments { get; set; }
    }
}
