using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/roadtypes")]
    [ApiController]
    public class RoadtypeController(IRoadTypeUseCase roadService): ControllerBase
    {
        private readonly IRoadTypeUseCase _roadTypeService = roadService;

        [HttpGet]
        public async Task<IActionResult> GetAllRoadType()
        {
            var result = await _roadTypeService.GetAllRoadType();
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoadTypeDetail([FromRoute] Guid id)
        {
            var result = await _roadTypeService.GetRoadTypeById(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoadType([FromBody] RoadTypeCreationDTO road_type)
        {
            var result = await _roadTypeService.CreateRoadType(road_type);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoadType([FromRoute] Guid id, [FromBody] RoadTypeCreationDTO road_type)
        {
            var result = await _roadTypeService.UpdateRoadType(id, road_type);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoadType([FromRoute] Guid id)
        {
            var result = await _roadTypeService.DeleteRoadType(id);
            return result.ToActionResult();
        }
    }
}
