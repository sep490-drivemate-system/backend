using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Commons.DTOs.Category;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> GetCategory(Guid id);
        Task<Result<IEnumerable<CategoryDto>>> GetCategories();
        Task<Result<CategoryDto>> CreateCategory(CategoryDTO categoryDTO);
        Task<Result<CategoryDto>> UpdateCategory(Guid id, UpdateCategoryDTO updateCategoryDTO);
        Task<Result<bool>> DeleteCategory(Guid id);
    }
}

