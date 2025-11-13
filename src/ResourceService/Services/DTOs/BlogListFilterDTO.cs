using SharedLibrary.SharedKernel.Pagination;

namespace ResourceService.Services.DTOs
{
    // DTO cho các API cần filter theo Status từ FE 
    public class BlogListFilterDTO : BlogListFilterBaseDTO
    {
        public int? Status { get; set; }
    }
}

