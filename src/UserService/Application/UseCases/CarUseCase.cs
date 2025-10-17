using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Dynamic;
using UserService.Application.Commons.DTOs.Cars;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class CarUseCase(IUnitOfWork unitOfWork): ICarUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<CarDetailDTO>> GetCarDetail(Guid id)
        {
            var car_info = await _unitOfWork.CarRepository.GetByIdAsync(id);

            if (car_info == null || car_info.IsDeleted)
            {
                return Result<CarDetailDTO>.Failure(ServiceError.NotFoundError("can not find the requested resource"), $"can not find car information for {id}");
            }

            CarDetailDTO parsed_car_info = new CarDetailDTO
            {
                Id = id,
                ModelName = car_info.Name,
                ThumbnailUrl = car_info.Thumbnail,
                Detail = car_info.Description,
                SeatCounts = car_info.Seat,
                LicensePlate = car_info.VehicleRegistration,
                UnitPrice = 0, // Requires changing domain model!
                ManufacturerName = car_info.Manufacturer.Name,
                FuelType = car_info.Fuel,
                VehicleType = car_info.CartType,
                OwnerId = car_info.InstructorId,
                BookingCount = 0, // Requires calling to booking service!
                AverageRating = 0, // Requires calling to booking service!
                PreferredLocation = "", // Requires changing domain model!
                CarPackages = new List<CarPackageDTO>(), // Requires calling to booking service!
                Images = car_info.CarImages.Where(x => !x.IsDeleted).Select(x => x.ImageUrl).ToList(),
            };

            return Result<CarDetailDTO>.Success(parsed_car_info, "success");
        }

        public async Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarFilterDTO filter)
        {
            var cars = await _unitOfWork.CarRepository.GetAllAsync();

            var filtered_cars = cars
                .Where(x => (filter.Manufacturer.Split(",").Contains(x.Manufacturer.Name) || string.IsNullOrEmpty(filter.Manufacturer)) && 
                (filter.SeatCounts.Split(",").Select(z => int.Parse(z)).Contains(x.Seat) || string.IsNullOrEmpty(filter.SeatCounts)) &&
                (filter.CarType.Split(",").Contains(x.CartType) || string.IsNullOrEmpty(filter.CarType)) &&
                (filter.FuelType.Split(",").Contains(x.Fuel) || string.IsNullOrEmpty(filter.FuelType))).ToList();

            var parsed_cars = filtered_cars.Select(x => new CarDTO
            {
                Id = x.Id,
                ModelName = x.Name,
                ThumbnailUrl = x.Thumbnail,
                SeatCounts = x.Seat,
                UnitPrice = 0, // Requires changing domain model!
                FuelType = x.Fuel,
                VehicleType = x.CartType,
                BookingCount = 0, // Requires calling to booking service!
                AverageRating = 0, // Requires calling to booking service!
                PreferredLocation = "", // Requires changing domain model!
            }).AsQueryable();

            PaginatedList<CarDTO> paginated_filtered_cars = PaginatedList<CarDTO>.Create<CarDTO>(parsed_cars, filter.PageIndex, filter.PageSize);

            return Result<PaginatedList<CarDTO>>.Success(paginated_filtered_cars, "success");
        }

        public async Task<Result<List<CarDTO>>> GetInstructorCarList(Guid id)
        {
           var cars = await _unitOfWork.CarRepository.GetAllAsync();

            var filtered_car = cars.Where(x => x.InstructorId == id);

            var parsed_cars = filtered_car.Select(x => new CarDTO
            {
                Id = x.Id,
                ModelName = x.Name,
                ThumbnailUrl = x.Thumbnail,
                SeatCounts = x.Seat,
                UnitPrice = 0, // Requires changing domain model!
                FuelType = x.Fuel,
                VehicleType = x.CartType,
                BookingCount = 0, // Requires calling to booking service!
                AverageRating = 0, // Requires calling to booking service!
                PreferredLocation = "", // Requires changing domain model!
            }).ToList();

            return Result<List<CarDTO>>.Success(parsed_cars);
        }

        public Task<Result<List<CarDTO>>> GetRecommendedCarList(int max_count = 5)
        {
            // Requires booking microservice!
            throw new NotImplementedException();
        }
    }
}
