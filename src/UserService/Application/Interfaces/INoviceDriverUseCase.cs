using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Users;

namespace UserService.Application.Interfaces
{
    public interface INoviceDriverUseCase
    {
       // Task<Result<IEnumerable<UserAddressDTO>>> GetNoviceDriverAddres(Guid id);
        Task<Result<NoviceDriverInfoFeedbackDTO>> GetNoviceDriverInfoForFeedback(Guid noviceDriverId);
    }
}
