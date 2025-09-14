using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Create()
        {
            return Ok();
        }

    }
}
