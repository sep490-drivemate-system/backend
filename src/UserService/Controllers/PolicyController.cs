using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
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
    }
}
