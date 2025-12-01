using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Domain.Enum;

namespace UserService.Application.Interfaces
{
    public interface IPolicyUseCase
    {
        Task<Result<IEnumerable<PolicyDTO>>> GetAllPolicy(PolicyType? policyType);
        Task<Result<bool>> AddNewPolicies(IEnumerable<PolicyCreationDTO> policies);
        Task<Result<bool>> UpdatePolicy(Guid id, PolicyCreationDTO policy);
        Task<Result<bool>> DeletePolicies(IEnumerable<Guid> policy_ids);
    }
}
