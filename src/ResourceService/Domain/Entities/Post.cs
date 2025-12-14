using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class Post : BaseEntites
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus Status { get; set; }
        public Guid AuthorId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Relationships
        public virtual ICollection<PostImage>? Images { get; set; }
        public virtual ICollection<PostVideo>? Videos { get; set; }
        public virtual ICollection<PostComment>? Comments { get; set; }
        public virtual ICollection<PostReaction>? Reactions { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
        public virtual ICollection<Category>? Categories{ get; set; }
        public virtual ICollection<PostReview>? Reviews { get; set; }
    }
}
