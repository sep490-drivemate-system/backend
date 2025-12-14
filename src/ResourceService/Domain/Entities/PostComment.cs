using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class PostComment : BaseEntites
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Navigation
        public virtual Post? Post { get; set; }
        public virtual PostComment? ParentComment { get; set; }
        public virtual ICollection<PostComment>? Replies { get; set; }
    }
}

