using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class PolicyUseCase(IUnitOfWork unitofwork): IPolicyUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitofwork;

        public async Task<Result<List<PolicyDTO>>> GetAllPolicy()
        {
            var policies = await _unitOfWork.PolicyRepository.GetAllAsync();

            return Result<List<PolicyDTO>>.Success(policies.Select(x => new PolicyDTO
            {
                Id = x.Id,
                Title = x.Name,
                Detail = x.Description,
                Type = x.PolicyType,
            }).ToList());
        }
    }
}
