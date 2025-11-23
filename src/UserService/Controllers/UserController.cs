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

        [HttpGet("{id}/address")]
        public async Task<IActionResult> GetAllUserSavedAddress([FromRoute]Guid id)
        {
            var result = await _userUseCase.GetUserSavedAddress(id);
            return result.ToActionResult();
        }

        [HttpGet("{id}/emergency-contact")]
        public async Task<IActionResult> GetAllUserSavedContact([FromRoute] Guid id)
        {
            var result = await _userUseCase.GetUserEmergencyContacts(id);
            return result.ToActionResult();
        }


        [HttpPost("{id}/emergency-contact")]
        public async Task<IActionResult> CreateNewSavedContact([FromRoute] Guid id, [FromBody] EmergencyContactDTO emergency_contact)
        {
            //var user_id = await _jwtService.ExtractUserIdFromToken(Request.Headers.Authorization[0]);
            var result = await _userUseCase.CreateUserEmergencyContacts(id, emergency_contact);
            return result.ToActionResult();
        }

        [HttpPost("{id}/saved-location")]
        public async Task<IActionResult> CreateNewSavedLocation([FromRoute] Guid id, [FromBody] UserAddressDTO address)
        {
            //var user_id = await _jwtService.ExtractUserIdFromToken(Request.Headers.Authorization[0]);
            var result = await _userUseCase.CreateUserSavedAddress(id, address);
            return result.ToActionResult();
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetUserStatistic(UserStatisticFilterDTO filter)
        {
            var result = await _userUseCase.GetUsersStatistic(filter);
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
