using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/roadtypes")]
    [ApiController]
    public class RoadtypeController(IRoadTypeService roadService): ControllerBase
    {
        private readonly IRoadTypeService _roadTypeService = roadService;

        [HttpGet()]
        public async Task<IActionResult> GetDrivingSkill()
        {
            var result = await _roadTypeService.GetAllRoadType();

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrivingSkillDetail([FromRoute] Guid id)
        {
            var result = await _roadTypeService.GetRoadTypeById(id);

            return result.ToActionResult();
        }
    }
}
