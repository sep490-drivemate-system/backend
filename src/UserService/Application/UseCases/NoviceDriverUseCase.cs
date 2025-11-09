using AutoMapper;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.NoviceDriver;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class NoviceDriverUseCase(IUnitOfWork unitOfWork,IMapper mapper): INoviceDriverUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<UserAddressDTO>>> GetNoviceDriverAddres(Guid id)
        {
            var driver = await _unitOfWork.NoviceDriverRepository.GetByIdAsync(id);

            if (driver == null)
            {
                return Result<List<UserAddressDTO>>
                    .Failure(ServiceError.NotFoundError(Commons.Constants.Messages.Common.NotFoundError));
            }

            return Result<List<UserAddressDTO>>.Success( driver.SavedLocations.Select(x => new UserAddressDTO
            {
                Id = x.Id,
                AddressString = x.DisplayName,
                Latitude = x.LocationLatitude,
                Longitude = x.LocationLongtitude
            }).ToList());
        }

        public async Task<Result<NoviceDriverInfoFeedbackDTO>> GetNoviceDriverInfoForFeedback(Guid noviceDriverId)
        {
            var driver = await _unitOfWork.NoviceDriverRepository.GetByIdWithUserAsync(noviceDriverId);

            if (driver == null)
            {
                return Result<NoviceDriverInfoFeedbackDTO>
                    .Failure(ServiceError.NotFoundError(Commons.Constants.Messages.Common.NotFoundError));
            }

            var result = new NoviceDriverInfoFeedbackDTO
            {
                Name = driver.User?.Fullname ?? "Unknown",
                Avatar = driver.User?.Avatar ?? string.Empty
            };

            return Result<NoviceDriverInfoFeedbackDTO>.Success(result);
        }

        
        
    }
}
