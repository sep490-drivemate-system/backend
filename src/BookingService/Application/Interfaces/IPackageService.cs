using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IPackageService
    {
        Task<Result<IEnumerable<Package>>> GetAllPackagesAsync();
        Task<Result<Package?>> GetPackageByIdAsync(Guid id);
        Task<Result<Package>> CreatePackageAsync(Package package);
        Task<Result<Package>> UpdatePackageAsync(Package package);
        Task<Result<bool>> DeletePackageAsync(Guid id);
    }
}
