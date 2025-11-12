using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Application.Interfaces;
using UserService.Domain.Enum;

namespace UserService.Controllers
{
    [Route("api/policy")]
    [ApiController]
    public class PolicyController(IPolicyUseCase usecases): ControllerBase
    {
        private readonly IPolicyUseCase _policyUseCase = usecases;

        [HttpGet]
        public async Task<IActionResult> GetAllPolicies(PolicyType policyType)
        {
            var result = await _policyUseCase.GetAllPolicy(policyType);
            return result.ToActionResult();
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateNewPolicies([FromBody] IEnumerable<PolicyCreationDTO> policies)
        {
            var result = await _policyUseCase.AddNewPolicies(policies);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy([FromRoute] Guid id,  [FromBody] PolicyCreationDTO policy)
        {
            var result = await _policyUseCase.UpdatePolicy(id, policy);
            return result.ToActionResult();
        }

        [HttpDelete("batch")]
        public async Task<IActionResult> DeletePolicy([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _policyUseCase.DeletePolicies(ids);
            return result.ToActionResult();
        }
    }
}
