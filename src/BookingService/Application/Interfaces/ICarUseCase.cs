using BookingService.Application.Commons.DTOs.Car_Documents;
using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using BookingService.Application.Commons.DTOs.Feedbacks;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface ICarUseCase
    {
        #region Car
        Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarListFilterDTO filter);
        Task<Result<CarDetailDTO>> GetCarDetail(Guid id);
        Task<Result<Guid>> CreateNewCar(CarCreationDTO information);
        Task<Result<bool>> UpdateCarInformation(Guid id, CarUpdateDTO information);
        Task<Result<bool>> DeleteCar(Guid id);
        #endregion

        #region Car document
        Task<Result<IEnumerable<CarDocumentViewDTO>>> GetCarDocuments(Guid car_id);
        Task<Result<CarDocumentViewDTO>> GetCarDocument(Guid car_id, string type);
        Task<Result<bool>> UploadCarDocument(Guid car_id, CarDocumenDTO document); 
        Task<Result<bool>> DeleteCarDocument(Guid car_id, string type);
        #endregion

        Task<Result<IEnumerable<CarDTO>>> GetAllCarsForPackage(Guid package_id);
        Task<Result<IEnumerable<CarFeedbackDTO>>> GetCarFeedback(Guid car_id);
        Task<Result<IEnumerable<CarDTO>>> GetRecommendedCarList(int max_count = 5);
        Task<Result<IEnumerable<CarInstructorDetailDTO>>> GetInstructorCarsList(Guid id);
        Task<Result<IEnumerable<CarDetailDTO>>> GetInstructorCarList(Guid id);
        Task<Result<bool>> ModerateInstructorCar(Guid car_id, string action);
    }
}
