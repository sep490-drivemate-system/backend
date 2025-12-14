using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class Tag : BaseEntites
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Navigation
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<QaQuestion>? QaQuestions{ get; set; }
    }
}

