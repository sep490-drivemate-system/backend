using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignIn()
        {
            return  Ok();
        }
        [HttpPost]
        public IActionResult SignUp()
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult SignInGoogle()
        {
            return Ok();
        }
    }
}
