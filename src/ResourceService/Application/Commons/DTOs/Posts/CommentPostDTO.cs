namespace ResourceService.Application.Commons.DTOs.Posts
{
    public class CommentPostDTO
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}

