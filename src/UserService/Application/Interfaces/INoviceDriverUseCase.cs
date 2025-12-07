using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.NoviceDriver;
using UserService.Application.Commons.DTOs.Users;

namespace UserService.Application.Interfaces
{
    public interface INoviceDriverUseCase
    {
        Task<Result<bool>> RegistratingNoviceDriverAccount(NoviceDriverRegistrationDTO registration_info);
        Task<Result<bool>> HasValidDrivingLicenseAsync(Guid noviceDriverId);
        Task<Result<NoviceDriverInfoFeedbackDTO>> GetNoviceDriverInfoForFeedback(Guid noviceDriverId);
        Task<Result<string>> UpdateNoviceDriverDrivingLicense(Guid id, IFormFile driving_license);
    }
}
