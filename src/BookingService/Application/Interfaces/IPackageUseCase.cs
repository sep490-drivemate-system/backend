using BookingService.Application.Commons.DTOs.Package;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IPackageUseCase
    {
        Task<Result<PaginatedList<PackageDTO>>> GetAllPackagesAsync(PackageListFilterDTO filter);
        Task<Result<Package?>> GetPackageByIdAsync(Guid id);
        Task<Result<Booking>> BuyPackageAsync(PackageBuyingDTO packageBuyingDTO,Guid guid);
        Task<Result<bool>> CreatePackageAsync(PackageCreationDTO package);
        Task<Result<Package>> UpdatePackageAsync(Package package);
        Task<Result<bool>> DeletePackageAsync(Guid id);
        Task<Result<List<PackageDto>>> GetInstructorPackagesAsync(Guid instructorId);
    }
}
