using BookingService.Application.Commons.DTOs.InstructorRoutes;
using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/instructor-route")]
    [ApiController]
    public class InstructorRoutesController : ControllerBase
    {
        private readonly IInstructorRoutesUseCase _useCase;
        private readonly IJwtService _jwtService;

        public InstructorRoutesController(IInstructorRoutesUseCase useCase, IJwtService jwtService)
        {
            _useCase = useCase;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Instructor) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> GetRoutes([FromQuery] Guid instructorId)
        {
            var result = await _useCase.GetRoutes(instructorId);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> GetRoute(Guid id)
        {
            var result = await _useCase.GetRoute(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Instructor) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateRoute([FromBody] InstructorRouteDTO instructorRouteDTO)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _useCase.CreateRoute(instructorRouteDTO, instructorId);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateRoute(Guid id, [FromBody] InstructorRouteDTO instructorRouteDTO)
        {
            var result = await _useCase.UpdateRoute(id, instructorRouteDTO);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteRoute(Guid id)
        {
            var result = await _useCase.DeleteRoute(id);
            return result.ToActionResult();
        }
    }
}
