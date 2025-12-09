using SharedLibrary.SharedKernel.Pagination;

namespace ResourceService.Application.Commons.DTOs
{
    // Base DTO cho các API không cần filter theo Status
    public class BlogListFilterBaseDTO : PaginationFilter
    {
        public string? SearchKey { get; set; }
        public Guid? CategoryId { get; set; }
    }
}

