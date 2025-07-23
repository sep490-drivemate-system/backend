using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAction()
        {
            var list = new List<string>{
               "1",
               "2",
            };
           return Ok(list);
          
        }
    }
}
