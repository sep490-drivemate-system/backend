using SharedLibrary.SharedKernel.Pagination;

namespace BookingService.Application.Commons.DTOs.Package
{
    public class PackageListFilterDTO : PaginationFilter
    {
        public string SearchKey { get; set; } = string.Empty;
        public bool? AllowSelfCar { get; set; } = null;
        public IEnumerable<Guid>? RoadTypes { get; set; } = null;
        public IEnumerable<Guid>? DrivingSkills { get; set; } = null;
    }
}
