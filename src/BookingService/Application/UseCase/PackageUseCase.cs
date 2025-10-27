using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;
using SharedLibrary.SharedKernel.Http.DTOs.Package;

namespace BookingService.Application.UseCase
{
    public class PackageUseCase : IPackageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageUseCase(IUnitOfWork unitOfWork)
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

        public async Task<Result<PackageResponse>> GetInstructorPackagesAsync(Guid instructorId)
        {
            // Get packages with PackageType included
            var allPackages = await _unitOfWork.PackageRepository.GetAllAsync();
            var instructorPackages = allPackages.Where(p => p.InstructorId == instructorId && !p.IsDeleted);

            var packageDtos = instructorPackages.Select(p => new PackageDto
            {
                Id = p.Id,
                Price = p.Price,
                TypeRental = (int)p.TypeRental,
                PackageTypeId = p.PackageTypeId,
                InstructorId = p.InstructorId,
                CarId = p.CarId,
                PackageTypeName = p.PackageType?.Name ?? string.Empty,
                Description = p.PackageType?.Description ?? string.Empty
            }).ToList();

            var response = new PackageResponse
            {
                Packages = packageDtos
            };

            return Result<PackageResponse>.Success(response);
        }
    }
}
