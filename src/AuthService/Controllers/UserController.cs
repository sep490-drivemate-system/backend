using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var listUser = new List<string>
            {
                "1111",
                "2222"
            };
            return Ok(listUser);
        }
    }
}
