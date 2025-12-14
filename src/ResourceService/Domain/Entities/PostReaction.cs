using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class PostReaction : BaseEntites
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType Type { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Navigation
        public virtual Post? Post { get; set; }
    }
}

