using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.Interfaces;

namespace ResourceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        private readonly IResourcesService _resourcesService;

        public ResourceController(ILogger<ResourceController> logger, IResourcesService resourcesService)
        {
            _logger = logger;
            _resourcesService = resourcesService;
        }

        [HttpGet("blogs")]
        public async Task<IActionResult> GetBlogs()
        {
            _logger.LogInformation("Getting blogs list");
            var resources = await _resourcesService.GetBlogsAsync();
            return Ok(new { success = true, data = resources });
        }

        [HttpGet("blogs/paged")]
        public async Task<IActionResult> GetBlogsPaged(
            [FromQuery] int page, 
            [FromQuery] int pageSize
            )
        {
            _logger.LogInformation("Getting blogs list with pagination - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limit max page size

            var result = await _resourcesService.GetBlogsPagedAsync(page, pageSize);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("blogs/{id}")]
        public async Task<IActionResult> GetBlogDetail([FromRoute] Guid id)
        {
            _logger.LogInformation("Getting blog detail for {Id}", id);
            var result = await _resourcesService.GetBlogDetailAsync(id);
            if (result == null) return NotFound(new { success = false, message = "Blog not found" });
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource(int id)
        {
            _logger.LogInformation("Getting resource with ID: {ResourceId}", id);
            
            // TODO: Implement actual resource retrieval logic
            var resource = new { Id = id, Name = $"Resource {id}", Type = "Document" };

            return Ok(new { success = true, data = resource });
        }

        [HttpPost]
        public async Task<IActionResult> CreateResource([FromBody] object resourceData)
        {
            _logger.LogInformation("Creating new resource");
            
            // TODO: Implement actual resource creation logic
            var newResource = new { Id = 3, Name = "New Resource", Type = "Document" };

            return CreatedAtAction(nameof(GetResource), new { id = 3 }, new { success = true, data = newResource });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource(int id, [FromBody] object resourceData)
        {
            _logger.LogInformation("Updating resource with ID: {ResourceId}", id);
            
            // TODO: Implement actual resource update logic
            var updatedResource = new { Id = id, Name = $"Updated Resource {id}", Type = "Document" };

            return Ok(new { success = true, data = updatedResource });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            _logger.LogInformation("Deleting resource with ID: {ResourceId}", id);
            
            // TODO: Implement actual resource deletion logic
            
            return Ok(new { success = true, message = $"Resource {id} deleted successfully" });
        }
    }
}
