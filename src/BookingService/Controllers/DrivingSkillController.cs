using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrivingSkillController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrivingSkill()
        {
            
            return Ok();
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
