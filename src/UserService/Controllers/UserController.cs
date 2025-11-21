using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Users;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserUseCase userUseCase, IJwtService jwtService): ControllerBase
    {
        private readonly IUserUseCase _userUseCase = userUseCase;
        private readonly IJwtService _jwtService = jwtService;

        [HttpGet("address")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> GetAllUserSavedAddress()
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _userUseCase.GetUserSavedAddress(userId);
            return result.ToActionResult();
        }

        [HttpGet("{id}/emergency-contact")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> GetAllUserSavedContact([FromRoute] Guid id)
        {
            var result = await _userUseCase.GetUserEmergencyContacts(id);
            return result.ToActionResult();
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetUserStatistic(UserStatisticFilterDTO filter)
        {
            var result = await _userUseCase.GetUsersStatistic(filter);
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _userUseCase.GetUser(id);
            return result.ToActionResult();
        }

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

        [HttpPost("batch-novice-driver-info")]
        public async Task<IActionResult> GetBatchNoviceDriverBasicInfo([FromBody] List<Guid> noviceDriverIds)
        {
            var result = await _userUseCase.GetBatchNoviceDriverBasicInfo(noviceDriverIds);
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
