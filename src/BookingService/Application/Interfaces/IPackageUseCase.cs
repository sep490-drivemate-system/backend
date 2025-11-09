using BookingService.Application.Commons.DTOs.Package;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IPackageUseCase
    {
        Task<Result<IEnumerable<Package>>> GetAllPackagesAsync();
        Task<Result<Package?>> GetPackageByIdAsync(Guid id);
        Task<Result<Package>> CreatePackageAsync(Package package);
        Task<Result<Booking>> BuyPackageAsync(PackageBuyingDTO packageBuyingDTO);
        Task<Result<Package>> UpdatePackageAsync(Package package);
        Task<Result<bool>> DeletePackageAsync(Guid id);
        Task<Result<List<PackageDto>>> GetInstructorPackagesAsync(Guid instructorId);
    }
}
