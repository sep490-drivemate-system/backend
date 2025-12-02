using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface ICarUseCase
    {
        Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarListFilterDTO filter);
        Task<Result<IEnumerable<CarDTO>>> GetRecommendedCarList(int max_count = 5);
        Task<Result<IEnumerable<CarInstructorDetailDTO>>> GetInstructorCarsList(Guid id);
        Task<Result<IEnumerable<CarDetailDTO>>> GetInstructorCarList(Guid id);
        Task<Result<IEnumerable<CarDetailDTO>>> GetInstructorCarWithUserId(Guid userId);
        Task<Result<CarDetailDTO>> GetCarDetail(Guid id);
        Task<Result<Guid>> CreateNewCar(CarCreationDTO information);
        Task<Result<bool>> UpdateCarInformation(Guid id, CarUpdateDTO information);
        Task<Result<bool>> DeleteCar(Guid id);
        Task<Result<bool>> ModerateInstructorCar(Guid car_id, string action);
    }
}
