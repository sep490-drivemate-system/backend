using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Configurations;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/configurations")]
    [ApiController]
    public class ConfigurationController(ISystemConfigurationUseCase SystemConfigurationUseCase) : ControllerBase
    {
        private ISystemConfigurationUseCase _configurationUseCase = SystemConfigurationUseCase;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConfiguration([FromRoute] Guid id)
        {
            var result = await _configurationUseCase.GetSystemConfigurationById(id);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConfiguration()
        {
            var result = await _configurationUseCase.GetSystemConfiguration();
            return result.ToActionResult();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateConfiguration([FromRoute] Guid id, [FromBody] UpdateConfigurationDTO updateDto)
        {
            var result = await _configurationUseCase.UpdateConfigurationValue(id, updateDto.NewValue, updateDto.NumberDate);
            return result.ToActionResult();
        }
    }
}
