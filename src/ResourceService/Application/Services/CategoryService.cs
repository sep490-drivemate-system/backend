using AutoMapper;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Commons.DTOs.Category;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Constants;
using ResourceService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Services
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDelete)
            {
                return Result<CategoryDto>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            var dto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync(
                filter: c => !c.IsDelete);

            var dtos = categories.Select(c => _mapper.Map<CategoryDto>(c));

            return Result<IEnumerable<CategoryDto>>.Success(dtos);
        }

        public async Task<Result<CategoryDto>> CreateCategory(CategoryDTO createCategoryDto)
        {
            if (string.IsNullOrWhiteSpace(createCategoryDto.Name))
            {
                return Result<CategoryDto>.Failure(ServiceError.BadRequestError(Messages.Category.NAME_REQUIRED));
            }

            var existingCategory = await _unitOfWork.Repository<Category>().GetAllAsync(
                filter: c => c.Name == createCategoryDto.Name && !c.IsDelete);
            
            if (existingCategory.Any())
            {
                return Result<CategoryDto>.Failure(ServiceError.ConflictError(Messages.Category.NAME_ALREADY_EXISTS));
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            await _unitOfWork.Repository<Category>().CreateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.Success(dto);
        }

        public async Task<Result<CategoryDto>> UpdateCategory(Guid id, UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDelete)
            {
                return Result<CategoryDto>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            if (!string.IsNullOrWhiteSpace(updateCategoryDTO.Name))
            {
                var existingCategory = await _unitOfWork.Repository<Category>().GetAllAsync(
                    filter: c => c.Name == updateCategoryDTO.Name && c.Id != id && !c.IsDelete);
                
                if (existingCategory.Any())
                {
                    return Result<CategoryDto>.Failure(ServiceError.ConflictError(Messages.Category.NAME_ALREADY_EXISTS));
                }

                category.Name = updateCategoryDTO.Name.Trim();
            }

            category.UpdateAt = DateTime.Now;
            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.Success(dto);
        }

        public async Task<Result<bool>> DeleteCategory(Guid id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDelete)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            category.IsDelete = true;
            category.UpdateAt = DateTime.UtcNow;
             _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}

