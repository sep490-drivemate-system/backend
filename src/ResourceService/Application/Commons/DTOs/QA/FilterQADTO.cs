using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Pagination;

namespace ResourceService.Application.Commons.DTOs.QA
{
    public class FilterQADTO : PaginationFilter
    {
        public string? Title { get; set; }
        public QaStatus? Status { get; set; }
    }
}

