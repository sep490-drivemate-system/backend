using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class DocumentType : BaseEntites
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<InstructorDocument> InstructorDocuments { get; set; } = new List<InstructorDocument>();
    }
}
