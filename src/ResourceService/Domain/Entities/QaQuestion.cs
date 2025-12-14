using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class QaQuestion : BaseEntites
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Navigation
        public virtual ICollection<QaAnswer>? Answers { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
    }
}

