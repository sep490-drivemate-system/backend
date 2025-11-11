using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;
using System.Text.Json;

namespace BookingService.Application.UseCase
{
    public class CarUseCase(IUnitOfWork unit_of_work, IHttpClientFactory http_client_factory, IMapper mapper) : ICarUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unit_of_work;
        private readonly IHttpClientFactory _http_client_factory = http_client_factory;
        private readonly IMapper _mapper = mapper;

        public Task<Result<Guid>> CreateNewCar(CarCreationDTO information)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> DeleteCar(Guid id)
        {
            var car_information = await _unitOfWork.CarRepository.GetByIdAsync(id);

            if (car_information == null)
            {
                return Result<bool>.Success(false, Messages.Commons.NOTFOUND);
            }

            try
            {
                _unitOfWork.CarRepository.Remove(car_information);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<CarDetailDTO>> GetCarDetail(Guid id)
        {
            var car_information = await _unitOfWork.CarRepository.GetByIdAsync(id);

            if (car_information == null)
            {
                return Result<CarDetailDTO>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            List<CarDocument> car_documents = new List<CarDocument>();

            if (!string.IsNullOrEmpty(car_information.DocumentJsonBlobString))
            {
                car_documents = JsonSerializer.Deserialize<List<CarDocument>>(car_information.DocumentJsonBlobString) ?? new List<CarDocument>();
            }

            return Result<CarDetailDTO>.Success(new CarDetailDTO
            {
                Id = car_information.Id,
                Detail = car_information.Description,
                FuelType = car_information.FuelType,
                LicensePlate = car_information.LicensePlate,
                ModelName = car_information.Name,
                SeatCounts = car_information.SeatCount,
                Images = car_information.CarImages?.Select(x => x.ImageUrl).ToList() ?? new List<string>(),
                ManufacturerName = car_information.Manufacturer?.Name ?? "Unknown",
                ThumbnailUrl = car_information.ThumbnailUrl,
                OwnerId = car_information.InstructorId,
                VehicleType = car_information.CarType,
                UnitPrice = car_information.Price,
                DocumentsRawString = car_information.DocumentJsonBlobString,
                Insurance = car_documents.FirstOrDefault(x => x.DocumentType == "insurance"), // Get from document blob string
                Registration = car_documents.FirstOrDefault(x => x.DocumentType == "registration"), // Get from document blob string
                BookingCount = car_information.Bookings?.Count ?? 0,
                AverageRating = car_information.Feedbacks?.Average(x => x.CarRating) ?? 0,
                Status = car_information.Status.ToString(),
            });
        }

        public async Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarListFilterDTO filter)
        {
            Expression<Func<Car, bool>>? filter_expression = x => (filter.Manufacturer == null || x.Manufacturer!.Name.StartsWith(filter.Manufacturer!))
            && (filter.LicenseTier == null || x.LicenseTier >= filter.LicenseTier)
            && (filter.SeatCounts == null || x.SeatCount == filter.SeatCounts)
            && (filter.CarType == null || x.CarType == filter.CarType)
            && !x.IsDeleted; ;
            Func<IQueryable<Car>, IOrderedQueryable<Car>>? order_expression = null;
            string included_properties = "Manufacturer,Packages,CarImages,Bookings,Feedbacks";


            if (filter.OrderBy != null)
            {
                switch (filter.OrderBy)
                {
                    case "price_asc":
                        order_expression = x => x.OrderBy(x => x.Price);
                        break;
                    case "price":
                        order_expression = x => x.OrderByDescending(x => x.Price);
                        break;
                    case "rating_asc":
                        order_expression = x => x.OrderBy(x => x.Feedbacks.Average(x => x.CarRating));
                        break;
                    case "rating":
                        order_expression = x => x.OrderByDescending(x => x.Feedbacks.Average(x => x.CarRating));
                        break;
                    case "booking_asc":
                        order_expression = x => x.OrderBy(x => x.Bookings.Count);
                        break;
                    case "booking":
                        order_expression = x => x.OrderByDescending(x => x.Bookings.Count);
                        break;
                    default:
                        break;
                }
            }

            var filtered_results = await _unitOfWork.CarRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties, disable_tracking: true);

            IEnumerable<CarDTO> mapped_results = filtered_results.Select(x => new CarDTO
            {
                Id = x.Id,
                ModelName = x.Name,
                ManufacturerName = x.Manufacturer?.Name ?? "Unknown",
                ThumbnailUrl = x.ThumbnailUrl,
                SeatCounts = x.SeatCount,
                FuelType = x.FuelType,
                VehicleType = x.CarType,
                UnitPrice = x.Price,
                LicenseTier = x.LicenseTier,
                BookingCount = x.Bookings?.Count ?? 0,
                AverageRating = x.Feedbacks?.Count > 0 ? x.Feedbacks.Average(x => x.CarRating) : 0,
            });

            return Result<PaginatedList<CarDTO>>.Success(PaginatedList<CarDTO>.Create(mapped_results, filter.PageIndex, filter.PageSize));
        }

        public async Task<Result<List<CarInstructorDetailDTO>>> GetInstructorCarList(Guid id)
        {
            Expression<Func<Car, bool>> filter_expression = x => x.InstructorId == id && !x.IsDeleted;
            Func<IQueryable<Car>, IOrderedQueryable<Car>> order_expression = x => x.OrderBy(u => u.CreatedAt);
            string included_properties = "Manufacturer,Packages,CarImages,Bookings,Feedbacks";

            var instructor_cars = await _unitOfWork.CarRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties, disable_tracking: true);

            var carDtos = _mapper.Map<List<CarInstructorDetailDTO>>(instructor_cars);

            return Result<List<CarInstructorDetailDTO>>.Success(carDtos, message: Messages.Commons.SUCCESS);
        }

        public Task<Result<List<CarDTO>>> GetRecommendedCarList(int max_count = 5)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateCarInformation(Guid id, CarUpdateDTO information)
        {
            throw new NotImplementedException();
        }
    }
}
