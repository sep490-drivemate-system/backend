using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.NoviceDriver;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/novice-driver")]
    [ApiController]
    public class NoviceDriverController(INoviceDriverUseCase usecases, ILogger<NoviceDriverController> logger ,IJwtService jwtService): ControllerBase
    {
        private readonly INoviceDriverUseCase _usecases = usecases;
        private readonly ILogger _logger = logger;
        private readonly IJwtService _jwtService = jwtService;

        [HttpPost("registration")]
        public async Task<IActionResult> RegisteringNoviceDriverAccount([FromBody] NoviceDriverRegistrationDTO info)
        {
            var result = await _usecases.RegistratingNoviceDriverAccount(info);
            return result.ToActionResult(logger);
        }

        [HttpGet("license-validity")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> GetDrivingLicenseValidity()
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _usecases.HasValidDrivingLicenseAsync(driverId);
            return result.ToActionResult(logger);
        }

        [HttpPut("{id}/license")]
        public async Task<IActionResult> UpdateDrivingLicense([FromRoute] Guid id, IFormFile image)
        {
            var result = await _usecases.UpdateNoviceDriverDrivingLicense(id, image);
            return result.ToActionResult(logger);
        }
    }
}
