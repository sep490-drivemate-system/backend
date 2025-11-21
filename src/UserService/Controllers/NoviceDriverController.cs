using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/novice-driver")]
    [ApiController]
    public class NoviceDriverController(INoviceDriverUseCase usecases,IJwtService jwtService): ControllerBase
    {
        private readonly INoviceDriverUseCase _usecases = usecases;
        private readonly IJwtService _jwtService = jwtService;

        //[HttpGet("address")]
        //[Authorize(Roles = nameof(UserRole.NoviceDriver))]
        //public async Task<IActionResult> GetAlDrvierAddress()
        //{
        //    var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
        //    var result = await _usecases.GetNoviceDriverAddres(driverId);
        //    return result.ToActionResult();
        //}
    }
}
