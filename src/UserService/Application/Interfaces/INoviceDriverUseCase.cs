using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;

namespace UserService.Application.Interfaces
{
    public interface INoviceDriverUseCase
    {
        //Task<Result<List<UserAddressDTO>>> GetNoviceDriverAddres(Guid id);
        Task<Result<NoviceDriverInfoFeedbackDTO>> GetNoviceDriverInfoForFeedback(Guid noviceDriverId);
    }
}
