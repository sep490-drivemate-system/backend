using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.DTOs.Instructors.Registration;
using UserService.Application.Interfaces;
using UserService.Domain.Enum;

namespace UserService.Controllers
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController(IInstructorUseCase usecase) : ControllerBase
    {
        private readonly IInstructorUseCase _usecase = usecase;

        [HttpGet]
        public async Task<IActionResult> GetInstructors([FromQuery] InstructorListFilterDTO filter)
        {
            var result = await _usecase.GetInstructors(filter);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorDetail([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorDetail(id);
            return result.ToActionResult();
        }

        [HttpGet("{id}/schedule")]
        public async Task<IActionResult> GetInstructorSchedule([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorSchedule(id);
            return result.ToActionResult();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsAnInstructor([FromForm] RegistrationDTO registration)
        {
            var result = await _usecase.RegisterInstructor(registration);
            return result.ToActionResult();
        }

        [HttpGet("applicants")]
        public async Task<IActionResult> GetInstructorApplicantsWithStatus([FromQuery] ApplicationStatus status)
        {
            var result = await _usecase.GetAllInstructorApplicationsByStatus(status);
            return result.ToActionResult();
        }

        [HttpGet("{id}/applicants")]
        public async Task<IActionResult> GetInstructorApplicants(Guid id)
        {
            var result = await _usecase.GetInstructorApplication(id);
            return result.ToActionResult();
        }

        [HttpGet("applicants/{id}")]
        public async Task<IActionResult> GetInstructorApplicantWithId(Guid id)
        {
            var result = await _usecase.GetApplicationById(id);
            return result.ToActionResult();
        }

        [HttpPost("applicants/{id}/inspection")]
        public async Task<IActionResult> ModerateActionForApplication([FromRoute] Guid id, [FromQuery] string action, [FromBody] InstructorNoteDTO note)
        {
            var result = await _usecase.ModerateInstructorApplication(id, action, note.Note);
            return result.ToActionResult();
        }

        [HttpPatch("{id}/applicants")]
        public async Task<IActionResult> UpdatePartialInstructorApplicant([FromRoute] Guid id, [FromForm] RegistrationDTO registration)
        {
            var result = await _usecase.UpdateInstructorApplication(id, registration);
            return result.ToActionResult();
        }
    }
}
