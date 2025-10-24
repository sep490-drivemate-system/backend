using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/novice-driver")]
    [ApiController]
    public class NoviceDriverController(INoviceDriverUseCase usecases): ControllerBase
    {
        private readonly INoviceDriverUseCase _usecases = usecases;

        [HttpGet("{id}/address")]
        public async Task<IActionResult> GetAlDrvierAddress([FromRoute] Guid id)
        {
            var result = await _usecases.GetNoviceDriverAddress(id);
            return result.ToActionResult();
        }
    }
}
