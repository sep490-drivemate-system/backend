using ResourceService.Domain.Enums;

namespace ResourceService.Application.Commons.DTOs.Posts
{
    public class PostsDTO
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorAvatar { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public List<Image> Images { get; set; }
        public List<Videos> Videos { get; set; }
        public PostStatus Status { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public List<ReactionDTO> Reactions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }

    public class CommentDTO
    {
        public Guid CommentId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatar { get; set; }
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReactionDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public ReactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class Image
    {
        public string Url { get; set; }
        public int? Order { get; set; }
    }
    public class Videos
    {
        public string Url { get; set; }
        public int Order { get; set; }
    }
}

