using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserUseCase userUseCase): ControllerBase
    {
        private readonly IUserUseCase _userUseCase= userUseCase;

        [HttpPost("ids")]
        public async Task<IActionResult> GetWithUserId([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _userUseCase.GetUserWithUserId(ids);
            return result.ToActionResult();
        }

        [HttpPost("instructor-ids")]
        public async Task<IActionResult> GetWithInstructorId([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _userUseCase.GetUserWithInstructorId(ids);
            return result.ToActionResult();
        }

        [HttpPost("driver-ids")]
        public async Task<IActionResult> GetWithDriverId([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _userUseCase.GetUserWithNoviceDriverId(ids);
            return result.ToActionResult();
        }

        [HttpPost("batch-instructor-info")]
        public async Task<IActionResult> GetBatchInstructorBasicInfo([FromBody] List<Guid> instructorIds)
        {
            var result = await _userUseCase.GetBatchInstructorBasicInfo(instructorIds);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDefaultUserAccount([FromBody] UserCreationDTO details)
        {
            var result = await _userUseCase.CreateDefaultUserAccount(details);
            return result.ToActionResult();
        }
    }
}
