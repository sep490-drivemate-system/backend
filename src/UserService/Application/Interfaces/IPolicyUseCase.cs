using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Domain.Enum;

namespace UserService.Application.Interfaces
{
    public interface IPolicyUseCase
    {
        Task<Result<List<PolicyDTO>>> GetAllPolicy();
    }
}
