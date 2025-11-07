using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly INoviceDriverUseCase _noviceDriverUseCase;

        public UserController(INoviceDriverUseCase noviceDriverUseCase)
        {
            _noviceDriverUseCase = noviceDriverUseCase;
        }

        [HttpPost("driver-feedback")]
        public async Task<IActionResult> GetNoviceDriverInfoForFeedback([FromBody] Guid noviceDriverId)
        {
            var result = await _noviceDriverUseCase.GetNoviceDriverInfoForFeedback(noviceDriverId);
            return result.ToActionResult();
        }
    }
}
