using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs;
using Services;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    /// <summary>
    /// API cho Inspector - Cần role Inspector
    /// </summary>
    [ApiController]
    [Route("api/inspector/blogs")]
    [Authorize(Roles = nameof(UserRole.Inspector))]
    public class InspectorBlogController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public InspectorBlogController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPendingBlogs([FromQuery] BlogListFilterDTO filter)
        {
            var result = await _serviceProviders.ResourcesService.GetPendingBlogsAsync(filter);
            return result.ToActionResult();
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.ApproveBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.RejectBlogAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/ban")]
        public async Task<IActionResult> BanBlog([FromRoute] Guid id)
        {
            var result = await _serviceProviders.ResourcesService.BanBlogAsync(id);
            return result.ToActionResult();
        }
    }
}

