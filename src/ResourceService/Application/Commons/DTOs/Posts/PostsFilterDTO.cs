using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Pagination;

namespace ResourceService.Application.Commons.DTOs.Posts
{
    public class PostsFilterDTO : PaginationFilter
    {
        public string? Title { get; set; }
        public string? Tag { get; set; }
        public string? Category { get; set; }
        public PostStatus? Status { get; set; }
    }   
}
