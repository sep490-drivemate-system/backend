using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs;
using ResourceService.Services.Interfaces;
using Services;
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
        private readonly IServiceProviders _serviceProviders;
        private readonly IJwtService _jwtService;

        public ResourceController(IServiceProviders serviceProviders, IJwtService jwtService)
        {
            _serviceProviders = serviceProviders;
            _jwtService = jwtService;
        }

        

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _serviceProviders.ResourcesService.GetCategoriesAsync();
            return result.ToActionResult();
        }


        [HttpGet("blogs/paged")]
        public async Task<IActionResult> GetBlogsPaged([FromQuery] BlogListFilterBaseDTO filter)
        {
            var result = await _serviceProviders.ResourcesService.GetBlogsPagedAsync(filter);
            return result.ToActionResult();
        }

        [HttpGet("blogs/{id}")]
        public async Task<IActionResult> GetBlogDetail([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.GetBlogDetailAsync(id);
             return result.ToActionResult();
        }




        [HttpGet("my-blogs")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> GetMyBlogs([FromQuery] BlogListFilterDTO filter)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.GetMyBlogsAsync(instructorId, filter);
            return result.ToActionResult();
        }

        [HttpGet("my-blogs/{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> GetMyBlogDetail([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.GetMyBlogDetailAsync(id, instructorId);
             return result.ToActionResult();
        }
        

        [HttpPost("blog-image")]
        public async Task<IActionResult> UploadBlogImage(IFormFile file)
        {
            var result = await _serviceProviders.ResourcesService.UploadImageForBlog(file);
            return result.ToActionResult();
        }

        [HttpPost("my-blogs")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDto createBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.CreateBlogAsync(createBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpPut("my-blogs/{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> UpdateBlog([FromRoute] Guid id, [FromBody] BlogUpdateDto updateBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.UpdateBlogAsync(id, updateBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpDelete("my-blogs/{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.DeleteBlogAsync(id, instructorId);
            return result.ToActionResult();
        }

        // Inspector APIs
        [HttpGet("blogs/pending")]
        //[Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> GetPendingBlogs([FromQuery] BlogListFilterBaseDTO filter)
        {
            var result = await _serviceProviders.ResourcesService.GetPendingBlogsAsync(filter);
            return result.ToActionResult();
        }

        [HttpPost("blogs/{id}/approve")]
        //[Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> ApproveBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.ApproveBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("blogs/{id}/reject")]
        //[Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> RejectBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.RejectBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("blogs/{id}/ban")]
        //[Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> BanBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.BanBlogAsync(id);
            return result.ToActionResult();
        }
    }
}
