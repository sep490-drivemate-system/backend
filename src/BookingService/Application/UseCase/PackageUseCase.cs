using BookingService.Application.Commons.Constants;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

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

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStacktrace:\n{ex.StackTrace}\nHelp link: {ex.HelpLink}");

                return Result<Package>.Failure(ServiceError.UnhandledException(Messages.Commons.UNHANDLED), Messages.Commons.UNHANDLED);
            }

            return Result<Package>.Success(createdPackage);

        }

        public async Task<Result<Package>> UpdatePackageAsync(Package package)
        {
            _unitOfWork.PackageRepository.Update(package);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStacktrace:\n{ex.StackTrace}\nHelp link: {ex.HelpLink}");

                return Result<Package>.Failure(ServiceError.UnhandledException(Messages.Commons.UNHANDLED), Messages.Commons.UNHANDLED);
            }

            return Result<Package>.Success(package);
        }

        public async Task<Result<bool>> DeletePackageAsync(Guid id)
        {
            _unitOfWork.PackageRepository.Remove(id);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStacktrace:\n{ex.StackTrace}\nHelp link: {ex.HelpLink}");

                return Result<bool>.Success(false, Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);

        }

        public async Task<Result<PackageResponse>> GetInstructorPackagesAsync(Guid instructorId)
        {
            //// Get packages with PackageType included
            //var allPackages = await _unitOfWork.PackageRepository.GetAllAsync();
            //var instructorPackages = allPackages.Where(p => p.InstructorId == instructorId && !p.IsDeleted);

            //var packageDtos = instructorPackages.Select(p => new PackageDto
            //{
            //    Id = p.Id,
            //    Price = p.Price,
            //    TypeRental = (int)p.TypeRental,
            //    PackageTypeId = p.PackageTypeId,
            //    InstructorId = p.InstructorId,
            //    CarId = p.CarId,
            //    PackageTypeName = string.Empty,
            //    Description = p.PackageType?.Description ?? string.Empty
            //}).ToList();

            //var response = new PackageResponse
            //{
            //    Packages = packageDtos
            //};

            //return Result<PackageResponse>.Success(response);

            Expression<Func<Package, bool>> filter_expression = x => x.InstructorId == instructorId && !x.IsDeleted;
            Func<IQueryable<Package>, IOrderedQueryable<Package>> order_expression = x => x.OrderBy(u => u.Name);
            string included_properties = "Cars";

            try
            {
                var packages = await _unitOfWork.PackageRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties, disable_tracking: true);

                return Result<PackageResponse>.Success(new PackageResponse
                {
                    Packages = packages.Select(x => new PackageDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        InstructorId = x.InstructorId,
                        CarIds = x.Cars.Select(x => x.Id).ToList(),
                    }).ToList(),
                });
            }
            catch (Exception)
            {
                {
                    return Result<PackageResponse>.Failure(ServiceError.UnhandledException(Messages.Commons.UNHANDLED), Messages.Commons.UNHANDLED);
                }
            }
        }
    }
}
