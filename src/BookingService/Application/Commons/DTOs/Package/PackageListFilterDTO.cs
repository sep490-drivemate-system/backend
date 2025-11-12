using SharedLibrary.SharedKernel.Pagination;

namespace BookingService.Application.Commons.DTOs.Package
{
    public class PackageListFilterDTO : PaginationFilter
    {
        public string SearchKey { get; set; } = string.Empty;
    }
}
