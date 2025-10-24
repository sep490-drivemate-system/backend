using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class DrivingSkillController(IDrivingSkillService skill_service): ControllerBase
    {
        private readonly IDrivingSkillService _skillService = skill_service;

        [HttpGet()]
        public async Task<IActionResult> GetDrivingSkill()
        {
            var result = await _skillService.GetAllDrivingSkills();

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrivingSkillDetail([FromRoute] Guid id)
        {
            var result = await _skillService.GetDrivingSkillWithId(id);

            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> DrivingSkill()
        {
          return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrivingSkill()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrivingSkill(Guid id)
        {
            return Ok();
        }

    }
}
