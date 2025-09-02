using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class InstructorApplication : BaseEntites
    {
        public string Note { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<InstructorDocument> InstructorDocuments { get; set; } = new List<InstructorDocument>();

    }
}
