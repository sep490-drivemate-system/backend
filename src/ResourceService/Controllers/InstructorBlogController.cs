using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs;
using Services;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    /// <summary>
    /// API cho Instructor - Cần role Instructor
    /// </summary>
    [ApiController]
    [Route("api/instructor/blogs")]
    [Authorize(Roles = nameof(UserRole.Instructor))]
    public class InstructorBlogController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IJwtService _jwtService;

        public InstructorBlogController(IServiceProviders serviceProviders, IJwtService jwtService)
        {
            _serviceProviders = serviceProviders;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyBlogs([FromQuery] BlogListFilterDTO filter)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.GetMyBlogsAsync(instructorId, filter);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMyBlogDetail([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.GetMyBlogDetailAsync(id, instructorId);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDto createBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.CreateBlogAsync(createBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog([FromRoute] Guid id, [FromBody] BlogUpdateDto updateBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.UpdateBlogAsync(id, updateBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.DeleteBlogAsync(id, instructorId);
            return result.ToActionResult();
        }
    }
}

