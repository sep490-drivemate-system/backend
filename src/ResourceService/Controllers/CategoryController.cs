using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Commons.DTOs.Category;
using ResourceService.Application.Interfaces;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IApplicationServiceProvider _serviceProviders;

        public CategoryController(IApplicationServiceProvider serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var result = await _serviceProviders.CategoryService.GetCategory(id);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _serviceProviders.CategoryService.GetCategories();
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            var result = await _serviceProviders.CategoryService.CreateCategory(categoryDTO);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            var result = await _serviceProviders.CategoryService.UpdateCategory(id, updateCategoryDTO);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _serviceProviders.CategoryService.DeleteCategory(id);
            return result.ToActionResult();
        }
    }
}
