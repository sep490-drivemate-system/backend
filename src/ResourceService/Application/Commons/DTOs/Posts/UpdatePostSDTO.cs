using ResourceService.Domain.Enums;

namespace ResourceService.Application.Commons.DTOs.Posts
{
    public class UpdatePostSDTO
    {
        public string? Reason { get; set; }
        public PostStatus? Status { get; set; }
    }
}
