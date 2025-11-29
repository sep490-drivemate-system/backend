using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class DrivingSkillController(IDrivingSkillUseCase skill_service): ControllerBase
    {
        private readonly IDrivingSkillUseCase _skillService = skill_service;

        [HttpGet]
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

        [HttpPost()]
        public async Task<IActionResult> CreateDrivingSkill([FromBody] DrivingSkillCreationDTO skill)
        {
            var result = await _skillService.CreateNewDrivingSkill(skill.SkillName);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrivingSkill([FromRoute] Guid id, [FromBody] DrivingSkillCreationDTO skill)
        {
            var result = await _skillService.UpdateDrivingSkill(id, skill);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrivingSkill(Guid id)
        {
            var result = await _skillService.DeleteDrivingSkills(id);
            return result.ToActionResult();
        }

    }
}
