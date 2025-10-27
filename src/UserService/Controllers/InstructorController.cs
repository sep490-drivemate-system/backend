using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/instructor")]
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
    }
}
