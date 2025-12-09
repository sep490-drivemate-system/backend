using SharedLibrary.SharedKernel.Pagination;

namespace ResourceService.Application.Commons.DTOs
{
    // DTO cho các API cần filter theo Status từ FE 
    public class BlogListFilterDTO : BlogListFilterBaseDTO
    {
        public int? Status { get; set; }
    }
}

