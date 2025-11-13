using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs;
using ResourceService.Services.Interfaces;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace ResourceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourcesService _resourcesService;
        private readonly IJwtService _jwtService;

        public ResourceController(IResourcesService resourcesService, IJwtService jwtService)
        {
            _resourcesService = resourcesService;
            _jwtService = jwtService;
        }

        

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _resourcesService.GetCategoriesAsync();
            return result.ToActionResult();
        }


        [HttpGet("blogs/paged")]
        public async Task<IActionResult> GetBlogsPaged([FromQuery] BlogListFilterDTO filter)
        {
            var result = await _resourcesService.GetBlogsPagedAsync(filter);
            return result.ToActionResult();
        }

        [HttpGet("blogs/{id}")]
        public async Task<IActionResult> GetBlogDetail([FromRoute] Guid id)
        {
            var result = await _resourcesService.GetBlogDetailAsync(id);
             return result.ToActionResult();
        }




        [HttpGet("my-blogs")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> GetMyBlogs()
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _resourcesService.GetMyBlogsAsync(instructorId);
            return  result.ToActionResult();
        }

        [HttpGet("my-blogs/{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> GetMyBlogDetail([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _resourcesService.GetMyBlogDetailAsync(id, instructorId);
             return result.ToActionResult();
        }
        

        [HttpPost("blog-image")]
        public async Task<IActionResult> UploadBlogImage(IFormFile file)
        {
            var result = await _resourcesService.UploadImageForBlog(file);
            return result.ToActionResult();
        }

        [HttpPost("my-blogs")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDto createBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _resourcesService.CreateBlogAsync(createBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpPut("my-blogs/{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> UpdateBlog([FromRoute] Guid id, [FromBody] BlogUpdateDto updateBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _resourcesService.UpdateBlogAsync(id, updateBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpDelete("my-blogs/{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _resourcesService.DeleteBlogAsync(id, instructorId);
            return result.ToActionResult();
        }
    }
}
