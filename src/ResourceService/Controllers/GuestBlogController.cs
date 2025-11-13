using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs;
using Services;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    /// <summary>
    /// API cho Guest - Không cần phân quyền
    /// </summary>
    [ApiController]
    [Route("api/guest/blogs")]
    public class GuestBlogController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public GuestBlogController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _serviceProviders.ResourcesService.GetCategoriesAsync();
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
    }
}

