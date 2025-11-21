using AutoMapper;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases
{
    public class NoviceDriverUseCase(IUnitOfWork unitOfWork,IMapper mapper): INoviceDriverUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        //public async Task<Result<IEnumerable<UserAddressDTO>>> GetNoviceDriverAddres(Guid id)
        //{
        //    var noviceDriver = await _unitOfWork.NoviceDriverRepository.GetByIdWithSavedLocationsAsync(id);

        //    if (noviceDriver == null)
        //    {
        //        return Result<IEnumerable<UserAddressDTO>>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
        //    }

        //    var addresses = _mapper.Map<IEnumerable<UserAddressDTO>>(noviceDriver.);

        //    return Result<IEnumerable<UserAddressDTO>>.Success(addresses);
        //}

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
