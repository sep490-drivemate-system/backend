using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Policy;

namespace UserService.Application.Interfaces
{
    public interface IPolicyUseCase
    {
        Task<Result<List<PolicyDTO>>> GetAllPolicy();
    }
}
