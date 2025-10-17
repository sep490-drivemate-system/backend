using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Cars;

namespace UserService.Application.Interfaces
{
    public interface ICarUseCase
    {
        Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarFilterDTO filter);

        Task<Result<List<CarDTO>>> GetRecommendedCarList(int max_count = 5);
        
        Task<Result<List<CarDTO>>> GetInstructorCarList(Guid id);

        Task<Result<CarDetailDTO>> GetCarDetail(Guid id);
    }
}
