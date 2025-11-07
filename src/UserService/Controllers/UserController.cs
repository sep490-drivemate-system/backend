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
        private readonly IUserUseCase _user_use_case = userUseCase;

        [HttpPost("ids")]
        public async Task<IActionResult> GetWithUserId([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _user_use_case.GetUserWithUserId(ids);
            return result.ToActionResult();
        }

        [HttpPost("instructor-ids")]
        public async Task<IActionResult> GetWithInstructorId([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _user_use_case.GetUserWithInstructorId(ids);
            return result.ToActionResult();
        }

        [HttpPost("driver-ids")]
        public async Task<IActionResult> GetWithDriverId([FromBody] IEnumerable<Guid> ids)
        {
            var result = await _user_use_case.GetUserWithNoviceDriverId(ids);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDefaultUserAccount([FromBody] UserCreationDTO details)
        {
            var result = await _user_use_case.CreateDefaultUserAccount(details);
            return result.ToActionResult();
        }
    }
}
