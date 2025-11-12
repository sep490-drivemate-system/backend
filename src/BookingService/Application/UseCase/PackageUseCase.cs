using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class PackageUseCase : IPackageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;
        private readonly IUser _userService;

        public PackageUseCase(IUnitOfWork unitOfWork, IMapper mapper, IPayment payment, IUser userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payment = payment;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<PackageDTO>>> GetAllPackagesAsync(PackageListFilterDTO filter)
        {
            try
            {
                var (packages, totalCount) = await _unitOfWork.PackageRepository.GetPackagesWithFilterAsync(
                    filter.SearchKey,
                    filter.PageNumber,
                    filter.PageSize);

                var instructorIds = packages
                    .Select(p => p.InstructorId)
                    .Distinct()
                    .ToList();

                // Fetch instructor info from UserService
                var instructorInfoDict = await _userService.GetBatchInstructorInfo(instructorIds);

                // Map to DTOs
                var packageDtos = packages.Select(p => new PackageDTO
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    InstructorName = instructorInfoDict.ContainsKey(p.InstructorId) 
                        ? instructorInfoDict[p.InstructorId].Fullname 
                        : "Unknown",
                    InstructorAvatar = instructorInfoDict.ContainsKey(p.InstructorId) 
                        ? instructorInfoDict[p.InstructorId].AvatarUrl 
                        : string.Empty,
                    HasVehicle = p.Cars != null && p.Cars.Any(),
                    Duration = (int)p.Duration,
                    RoadTypes = p.RoadTypes != null 
                        ? p.RoadTypes.Select(r => r.Name).ToList() 
                        : new List<string>(),
                    Skills = p.DrivingSkills != null 
                        ? p.DrivingSkills.Select(s => s.Name).ToList() 
                        : new List<string>(),
                    Price = p.Price,
                    BookingCount = p.Bookings?.Count ?? 0
                }).ToList();

                // Create paginated result
                var paginatedList = PaginatedList<PackageDTO>.CreateFromPagedData(
                    packageDtos,
                    filter.PageNumber,
                    filter.PageSize,
                    totalCount);

                return Result<PaginatedList<PackageDTO>>.Success(paginatedList);
            }
            catch (Exception ex)
            {
                return Result<PaginatedList<PackageDTO>>.Failure(
                    ServiceError.UnhandledException(ex.Message),
                    "Failed to retrieve packages");
            }
        }

        public async Task<Result<Package?>> GetPackageByIdAsync(Guid id)
        {
            var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);
            return Result<Package?>.Success(package);
        }

        public async Task<Result<bool>> CreatePackageAsync(PackageCreationDTO package)
        {
            // Get other resources required for package creation
            var package_driving_skills = await _unitOfWork.SkillRepository.GetAllAsync(filter: x => package.DrivingSkills.Contains(x.Id));
            var package_road_types = await _unitOfWork.RoadTypeRepository.GetAllAsync(filter: x => package.RoadTypes.Contains(x.Id));
            var package_cars = await _unitOfWork.CarRepository.GetAllAsync(filter: x => package.PackageCars.Contains(x.Id));

            // Create package
            Package packageEntity = new Package
            {
                Name = package.Name,
                Description = package.Description,
                Duration = package.Duration,
                ThumbnailUrl = "", // Default for no thumbnail.
                Price = package.Price,
                InstructorId = package.InstructorId,
                AllowNoviceVehicle = package.AllowNoviceCar,
                DrivingSkills = package_driving_skills,
                RoadTypes = package_road_types,
                Cars = package_cars,
            };

            var createdPackage = await _unitOfWork.PackageRepository.CreateAsync(packageEntity);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException(Messages.Commons.UNHANDLED), Messages.Commons.UNHANDLED);
            }


            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
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

        public async Task<Result<List<PackageDto>>> GetInstructorPackagesAsync(Guid instructorId)
        {
            var packages = await _unitOfWork.PackageRepository.GetInstructorPackages(instructorId);

            var packageDtos = _mapper.Map<List<PackageDto>>(packages);

            return Result<List<PackageDto>>.Success(packageDtos);
        }

        public async Task<Result<Booking>> BuyPackageAsync(PackageBuyingDTO packageBuyingDTO,Guid driverId)
        {
            Guid id = Guid.NewGuid();
            var walletCheckResponse = await _payment.CheckWalletBooking(
               driverId,
               packageBuyingDTO.PriceAtBuyingTime,
              id,null);

            if (!walletCheckResponse.IsPayment)
            {
                return Result<Booking>.Failure(ServiceError.BadRequestError(Messages.Booking.INSUFFICENTCREDIT));
            }
            var booking = _mapper.Map<Booking>(packageBuyingDTO);
            booking.Id = id;
            booking.Status = BookingStatus.Purchased;
            var createdBooking = await _unitOfWork.BookingRepository.CreateAsync(booking);
            await _unitOfWork.CommitChangesAsync();

            return Result<Booking>.Success(createdBooking);
            
        }
    }
}
