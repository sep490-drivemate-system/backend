using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.NoviceDriver;

namespace UserService.Application.Interfaces
{
    public interface INoviceDriverUseCase
    {
        Task<Result<IEnumerable<UserAddressDTO>>> GetNoviceDriverAddress(Guid id);
    }
}
