using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.NoviceDriver;

namespace UserService.Application.Interfaces
{
    public interface INoviceDriverUseCase
    {
        Task<Result<List<UserAddressDTO>>> GetNoviceDriverAddress(Guid id);
        Task<Result<NoviceDriverInfoFeedbackDTO>> GetNoviceDriverInfoForFeedback(Guid noviceDriverId);
    }
}
