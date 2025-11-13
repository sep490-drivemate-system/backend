using SharedLibrary.SharedKernel.Pagination;

namespace ResourceService.Services.DTOs
{
    public class BlogListFilterDTO : PaginationFilter
    {
        public string? SearchKey { get; set; }
        public Guid? CategoryId { get; set; }
    }
}

