using AutoMapper;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Application.Interfaces;
using UserService.Domain.Enum;

namespace UserService.Application.UseCases
{
    public class PolicyUseCase(IUnitOfWork unitofwork, IMapper mapper): IPolicyUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitofwork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<PolicyDTO>>> GetAllPolicy()
        {
            var policies = await _unitOfWork.PoliciesRepository.GetAllAsync();
            return Result<List<PolicyDTO>>.Success(policies.Select(x => new PolicyDTO
            {
                Id = x.Id,
                Title = x.Name,
                Detail = x.Description
            }).ToList());
        }
    }
}
