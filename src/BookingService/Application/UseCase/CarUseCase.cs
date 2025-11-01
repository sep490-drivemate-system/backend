using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class CarUseCase(IUnitOfWork unitOfWork): ICarUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public Task<Result<Guid>> CreateNewCar(CarCreationDTO information)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteCar(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CarDetailDTO>> GetCarDetail(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarListFilterDTO filter)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<CarDTO>>> GetInstructorCarList(Guid id)
        {
            throw new NotImplementedException();
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
