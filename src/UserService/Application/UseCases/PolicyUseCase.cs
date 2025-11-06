using AutoMapper;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enum;

namespace UserService.Application.UseCases
{
    public class PolicyUseCase(IUnitOfWork unitofwork, IMapper mapper): IPolicyUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitofwork;

        public async Task<Result<bool>> AddNewPolicies(IEnumerable<PolicyCreationDTO> policies)
        {
            foreach (var policy in policies)
            {
                await _unitOfWork.PoliciesRepository.CreateAsync(new Policy
                {
                    Name = policy.Title,
                    Description = policy.Description,
                });
            }

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Common.UnknownError);
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }

        public async Task<Result<IEnumerable<PolicyDTO>>> GetAllPolicy()
        {
            Expression<Func<Policy, bool>> filter_expression = x => !x.IsDeleted;

            var policies = await _unitOfWork.PoliciesRepository.GetAllAsync(filter: filter_expression);

            return Result<IEnumerable<PolicyDTO>>.Success(policies.Select(x => new PolicyDTO
            {
                Id = x.Id,
                Title = x.Name,
                Detail = x.Description
            }));
        }

        public async Task<Result<bool>> UpdatePolicy(Guid id, PolicyCreationDTO policy)
        {
            var policy_entity = await _unitOfWork.PoliciesRepository.GetByIdAsync(id);

            if (policy_entity == null || policy_entity.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            policy_entity.Name = policy.Title;
            policy_entity.Description = policy.Description;

            _unitOfWork.PoliciesRepository.Update(policy_entity);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true, Messages.Common.Success);
        }

        public async Task<Result<bool>> DeletePolicies(IEnumerable<Guid> policy_ids)
        {
            Expression<Func<Policy, bool>> filter_expression = x => !x.IsDeleted && policy_ids.Contains(x.Id);

            var policy_entity = await _unitOfWork.PoliciesRepository.GetAllAsync(filter: filter_expression);

            foreach (var policy in policy_entity)
            {
                _unitOfWork.PoliciesRepository.Remove(policy);
            }

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Common.UnknownError);
            }

            return Result<bool>.Success(true, Messages.Common.Success);
        }
    }
}
