using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Application.Commons.DTOs.Tags;
using ResourceService.Application.Interfaces;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    [Route("api/tag")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IApplicationServiceProvider _serviceProviders;

        public TagController(IApplicationServiceProvider serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(Guid id)
        {
            var result = await _serviceProviders.TagService.GetTag(id);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var result = await _serviceProviders.TagService.GetTags();
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDTO createTagDTO)
        {
            var result = await _serviceProviders.TagService.CreateTag(createTagDTO);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] UpdateTagDTO updateTagDTO)
        {
            var result = await _serviceProviders.TagService.UpdateTag(id, updateTagDTO);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var result = await _serviceProviders.TagService.DeleteTag(id);
            return result.ToActionResult();
        }
    }
}
