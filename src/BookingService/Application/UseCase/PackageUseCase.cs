using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;
using System.Net.Http;

namespace BookingService.Application.UseCase
{
    public class PackageUseCase : IPackageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;
        private readonly IUser _userService;

        public PackageUseCase(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IMapper mapper, IPayment payment, IUser userService)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _payment = payment;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<PackageDTO>>> GetAllPackagesAsync(PackageListFilterDTO filter)
        {
            try
            {
                Expression<Func<Package, bool>> filterExpression = x => !x.IsDeleted
                && (filter.SearchKey == null || x.Name.Contains(filter.SearchKey))
                && (filter.DrivingSkills == null || filter.DrivingSkills.Any(y => x.DrivingSkills.Any(z => z.Id == y)))
                && (filter.RoadTypes == null || filter.RoadTypes.Any(y => x.RoadTypes.Any(z => z.Id == y)))
                && (filter.AllowSelfCar == null || x.AllowNoviceVehicle == filter.AllowSelfCar);

                string includedProperties = "DrivingSkills,RoadTypes,Bookings,Cars";

                var filteredPackages = await _unitOfWork.PackageRepository.GetAllAsync(filter: filterExpression, include_properties: includedProperties);
                
                // Getting instructor information through User Service API.
                var instructorIdList = filteredPackages.Select(x => x.InstructorId).Distinct();

                var http_client = _httpClientFactory.CreateClient("UserServiceClient");
                var http_message = await http_client.PostAsJsonAsync<IEnumerable<Guid>>("api/users/ids", instructorIdList);
                var response = await http_message.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDetailDTO>>>();

                // Map the final result
                var mappedList = filteredPackages.Select(x => new PackageDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Duration = x.Duration,
                    Price = x.Price,
                    AllowSelfCar = x.AllowNoviceVehicle,
                    RoadTypes = x.RoadTypes.Select(x => x.Name),
                    Skills = x.DrivingSkills.Select(x => x.Name),
                    BookingCount = x.Bookings.Count(),
                    CarCount = x.Cars.Count(),
                    InstructorAvatar = response.Value.FirstOrDefault(y => y.UserId == x.InstructorId)?.AvatarUrl ?? "",
                    InstructorName = response.Value.FirstOrDefault(y => y.UserId == x.InstructorId)?.FullName ?? "Unknown",
                });

                // Pagination
                var paginatedList = PaginatedList<PackageDTO>.CreateFromPagedData(mappedList.ToList(), filter.PageNumber, filter.PageSize, mappedList.Count());

                return Result<PaginatedList<PackageDTO>>.Success(paginatedList);
            }
            catch (HttpRequestException ex)
            {
                return Result<PaginatedList<PackageDTO>>.Failure(ServiceError.ServiceUnavailableError($"UserServiceClient"), Messages.Commons.UNHANDLED);
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


    }
}
