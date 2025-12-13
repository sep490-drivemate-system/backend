using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Interfaces;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    /// <summary>
    /// API quản lý Blog - Gộp tất cả các endpoint cho Guest, Instructor và Inspector
    /// </summary>
    [ApiController]
    [Route("api/blogs")]
    public class BlogController : ControllerBase
    {
        private readonly IApplicationServiceProvider _serviceProviders;
        private readonly IJwtService _jwtService;

        public BlogController(IApplicationServiceProvider serviceProviders, IJwtService jwtService)
        {
            _serviceProviders = serviceProviders;
            _jwtService = jwtService;
        }

        #region Guest Endpoints (Không cần phân quyền)

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _serviceProviders.ResourcesService.GetCategoriesAsync();
            return result.ToActionResult();
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetBlogStatuses()
        {
            var result = await _serviceProviders.ResourcesService.GetBlogStatusesAsync();
            return result.ToActionResult();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetBlogsPaged([FromQuery] BlogListFilterBaseDTO filter)
        {
            var result = await _serviceProviders.ResourcesService.GetBlogsPagedAsync(filter);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogDetail([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.GetBlogDetailAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadBlogImage(IFormFile file)
        {
            var result = await _serviceProviders.ResourcesService.UploadImageForBlog(file);
            return result.ToActionResult();
        }

        #endregion

        #region Instructor Endpoints (Cần role Instructor)

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

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateBlog([FromForm] BlogCreateRequest request)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.CreateBlogAsync(request, instructorId);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> UpdateBlog([FromRoute] Guid id, [FromBody] BlogUpdateDto updateBlogDto)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.UpdateBlogAsync(id, updateBlogDto, instructorId);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid id)
        {
            var instructorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.ResourcesService.DeleteBlogAsync(id, instructorId);
            return result.ToActionResult();
        }

        #endregion

        #region Inspector Endpoints (Cần role Inspector)

        [HttpGet("list")]
        [Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> GetBlogsList([FromQuery] BlogListFilterDTO filter)
        {
            var result = await _serviceProviders.ResourcesService.GetBlogsListAsync(filter);
            return result.ToActionResult();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> ApproveBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.ApproveBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> RejectBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.RejectBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/ban")]
        [Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> BanBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.BanBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("categories")]
        [Authorize(Roles = nameof(UserRole.Inspector))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            var result = await _serviceProviders.ResourcesService.CreateCategoryAsync(createCategoryDto);
            return result.ToActionResult();
        }

        #endregion
    }
}

