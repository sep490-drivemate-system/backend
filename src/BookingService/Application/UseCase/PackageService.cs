using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<Package>>> GetAllPackagesAsync()
        {

                var packages = await _unitOfWork.PackageRepository.GetAllAsync();
                return Result<IEnumerable<Package>>.Success(packages);
           
        }

        public async Task<Result<Package?>> GetPackageByIdAsync(Guid id)
        {

                var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);
                return Result<Package?>.Success(package);
           
        }

        public async Task<Result<Package>> CreatePackageAsync(Package package)
        {

                var createdPackage = await _unitOfWork.PackageRepository.CreateAsync(package);
                return Result<Package>.Success(createdPackage);
          
        }

        public async Task<Result<Package>> UpdatePackageAsync(Package package)
        {

                var updatedPackage = await _unitOfWork.PackageRepository.Update(package);
                return Result<Package>.Success(updatedPackage);
           
        }

        public async Task<Result<bool>> DeletePackageAsync(Guid id)
        {

                var result = await _unitOfWork.PackageRepository.Remove(id);
                return Result<bool>.Success(result);
           
        }
    }
}
