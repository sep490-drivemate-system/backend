using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class QaAnswer : BaseEntites
    {
        public Guid QuestionId { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Navigation
        public virtual QaQuestion? Question { get; set; }
    }
}

