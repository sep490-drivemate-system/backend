using AutoMapper;
using Microsoft.AspNetCore.Http;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Constants;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace ResourceService.Application.Services
{
    public class ResourcesService: IResourcesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryServiceProvider _cloudinary;

        public ResourcesService(IUnitOfWork unitOfWork, ICloudinaryServiceProvider cloudinary, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Parse ImageList JSON string to List of image URLs
        /// </summary>
        private IList<string> ParseImageList(string imageListJson)
        {
            if (string.IsNullOrEmpty(imageListJson))
            {
                return new List<string>();
            }

            try
            {
                var imageList = JsonSerializer.Deserialize<List<string>>(imageListJson);
                return imageList ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<Result<ICollection<CategoryDto>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.ResourceRepository.GetCategoriesAsync();
                var categoryDtos = _mapper.Map<ICollection<CategoryDto>>(categories);
                return Result<ICollection<CategoryDto>>.Success(categoryDtos, Messages.Commons.SUCCESS);
            }
            catch (Exception)
            {
                return Result<ICollection<CategoryDto>>.Failure(
                    ServiceError.UnhandledException(Messages.Blog.RETRIEVE_ERROR),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<ICollection<BlogStatusDto>>> GetBlogStatusesAsync()
        {
            try
            {
                var statuses = Enum.GetValues(typeof(BlogStatus))
                    .Cast<BlogStatus>()
                    .Select(s => new BlogStatusDto
                    {
                        Value = (int)s,
                        Name = s.ToString()
                    })
                    .ToList();

                return Result<ICollection<BlogStatusDto>>.Success(statuses, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<ICollection<BlogStatusDto>>.Failure(
                    ServiceError.UnhandledException($"Error retrieving blog statuses: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(createCategoryDto.Name))
                {
                    return Result<CategoryDto>.Failure(
                        ServiceError.BadRequestError(Messages.Category.NAME_REQUIRED),
                        Messages.Category.NAME_REQUIRED);
                }

                // Check if category name already exists
                var existingCategories = await _unitOfWork.ResourceRepository.GetCategoriesAsync();
                if (existingCategories.Any(c => c.Name.Trim().Equals(createCategoryDto.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    return Result<CategoryDto>.Failure(
                        ServiceError.BadRequestError(Messages.Category.NAME_ALREADY_EXISTS),
                        Messages.Category.NAME_ALREADY_EXISTS);
                }

                var category = _mapper.Map<Category>(createCategoryDto);
                category.Id = Guid.NewGuid();
                category.CreatedAt = DateTime.Now;
                category.IsDelete = false;

                await _unitOfWork.ResourceRepository.CreateCategory(category);
                var result = await _unitOfWork.SaveChangesAsync();
                
                if (result <= 0)
                {
                    return Result<CategoryDto>.Failure(
                        ServiceError.UnhandledException(Messages.Category.CREATE_FAILED),
                        Messages.Category.CREATE_FAILED);
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Result<CategoryDto>.Success(categoryDto, Messages.Category.CREATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<CategoryDto>.Failure(
                    ServiceError.UnhandledException($"{Messages.Category.CREATE_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<PaginatedList<ResourceDto>>> GetMyBlogsAsync(Guid instructorId, BlogListFilterDTO filter)
        {
            try
            {
                // Convert Status (int?) từ query string sang BlogStatus enum
                BlogStatus? statusEnum = null;
                if (filter.Status.HasValue && Enum.IsDefined(typeof(BlogStatus), filter.Status.Value))
                {
                    statusEnum = (BlogStatus)filter.Status.Value;
                }

                Expression<Func<Blog, bool>> filterExpression = x =>
                    x.InstructorId == instructorId &&
                    (string.IsNullOrEmpty(filter.SearchKey) || x.Title.Contains(filter.SearchKey)) &&
                    (!filter.CategoryId.HasValue || x.CategoryId == filter.CategoryId.Value) &&
                    (!statusEnum.HasValue || x.Status == statusEnum.Value) &&
                    !x.IsDelete;

                string includedProperties = "Category";

                var allBlogs = await _unitOfWork.ResourceRepository.GetAllBlogsAsync(
                    filter: filterExpression,
                    includeProperties: includedProperties
                );

                var paginatedBlogs = PaginatedList<Blog>.Create(allBlogs, filter.PageNumber, filter.PageSize);

                var blogDtos = _mapper.Map<List<ResourceDto>>(paginatedBlogs.PageContent.ToList());

                var paginatedResult = PaginatedList<ResourceDto>.CreateFromPagedData(
                    blogDtos,
                    paginatedBlogs.CurrentPage,
                    paginatedBlogs.PageSize,
                    paginatedBlogs.TotalCount
                );

                return Result<PaginatedList<ResourceDto>>.Success(paginatedResult, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<PaginatedList<ResourceDto>>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.RETRIEVE_ERROR}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<PaginatedList<ResourceDto>>> GetBlogsPagedAsync(BlogListFilterBaseDTO filter)
        {
            try
            {
                Expression<Func<Blog, bool>> filterExpression = x =>
                    (string.IsNullOrEmpty(filter.SearchKey) || x.Title.Contains(filter.SearchKey)) &&
                    (!filter.CategoryId.HasValue || x.CategoryId == filter.CategoryId.Value) &&
                    !x.IsDelete &&
                    x.Status == BlogStatus.Active; // Chỉ hiển thị blogs đã được duyệt

                string includedProperties = "Category";

                var allBlogs = await _unitOfWork.ResourceRepository.GetAllBlogsAsync(
                    filter: filterExpression,
                    includeProperties: includedProperties
                );

                var paginatedBlogs = PaginatedList<Blog>.Create(allBlogs, filter.PageNumber, filter.PageSize);

                var blogDtos = _mapper.Map<List<ResourceDto>>(paginatedBlogs.PageContent.ToList());

                var paginatedResult = PaginatedList<ResourceDto>.CreateFromPagedData(
                    blogDtos,
                    paginatedBlogs.CurrentPage,
                    paginatedBlogs.PageSize,
                    paginatedBlogs.TotalCount
                );

                return Result<PaginatedList<ResourceDto>>.Success(paginatedResult, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<PaginatedList<ResourceDto>>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.RETRIEVE_ERROR}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<BlogDetailDto>> GetBlogDetailAsync(Guid id)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(id);
                if (blog == null)
                {
                    return Result<BlogDetailDto>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                var blogDetail = _mapper.Map<BlogDetailDto>(blog);
                blogDetail.ImageList = ParseImageList(blog.ImageList);

                return Result<BlogDetailDto>.Success(blogDetail, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<BlogDetailDto>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.RETRIEVE_ERROR}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<BlogDetailDto>> GetMyBlogDetailAsync(Guid id, Guid instructorId)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetMyBlogDetailAsync(id, instructorId);
                if (blog == null)
                {
                    return Result<BlogDetailDto>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                var blogDetail = _mapper.Map<BlogDetailDto>(blog);
                blogDetail.ImageList = ParseImageList(blog.ImageList);

                return Result<BlogDetailDto>.Success(blogDetail, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<BlogDetailDto>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.RETRIEVE_ERROR}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> DeleteBlogAsync(Guid id, Guid instructorId)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(id);
                if (blog == null)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }
                if (blog.InstructorId != instructorId)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }
            var marked = await _unitOfWork.ResourceRepository.SoftDeleteBlogAsync(id);
                if (!marked)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.DELETE_FAILED),
                        Messages.Blog.DELETE_FAILED);
                }

                return Result<bool>.Success(true, Messages.Blog.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.DELETE_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> CreateBlogAsync(BlogCreateRequest request, Guid instructorId)
        {
            try
            {
                // Parse contents JSON if provided
                IList<BlogContentCreateDto> contentsList = null;
                if (!string.IsNullOrEmpty(request.Contents))
                {
                    try
                    {
                        contentsList = JsonSerializer.Deserialize<IList<BlogContentCreateDto>>(request.Contents);
                    }
                    catch
                    {
                        contentsList = new List<BlogContentCreateDto>();
                    }
                }

                // Upload thumbnail to Cloudinary
                string thumbnailUrl = null;
                if (request.Thumbnail != null && request.Thumbnail.Length > 0)
                {
                    thumbnailUrl = _cloudinary.UploadImageFormFileResourceToCloudinary(
                        request.Thumbnail,
                        $"blog_thumbnail_{instructorId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}"
                    );
                }

                // Upload images to Cloudinary and get URLs
                var imageUrls = new List<string>();
                if (request.Images != null && request.Images.Count > 0)
                {
                    foreach (var image in request.Images)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var imageUrl = _cloudinary.UploadImageFormFileResourceToCloudinary(
                                image, 
                                $"blog_{instructorId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}"
                            );
                            imageUrls.Add(imageUrl);
                        }
                    }
                }

                // Convert image URLs to JSON string
                var imageListJson = imageUrls.Count > 0 
                    ? JsonSerializer.Serialize(imageUrls) 
                    : null;

                // Map request to DTO
                var createBlogDto = new BlogCreateDto
                {
                    Title = request.Title,
                    ThumbnailUrl = thumbnailUrl ?? string.Empty,
                    CategoryId = request.CategoryId,
                    Contents = contentsList ?? new List<BlogContentCreateDto>()
                };

                // Map DTO to Entity using AutoMapper
                var blog = _mapper.Map<Blog>(createBlogDto);
                
                // Set additional properties
                blog.InstructorId = instructorId;
                blog.CreatedAt = DateTime.Now;
                blog.Status = BlogStatus.Pending; 
                blog.IsDelete = false;
                blog.ImageList = imageListJson;

                // Map contents using AutoMapper
                if (createBlogDto.Contents != null && createBlogDto.Contents.Any())
                {
                    blog.Contents = _mapper.Map<List<BlogContent>>(createBlogDto.Contents);
                    var now = DateTime.Now;
                    foreach (var content in blog.Contents)
                    {
                        content.BlogId = blog.Id;
                        content.CreatedAt = now;
                        content.UpdateAt = now;
                        content.IsDelete = false;
                    }
                }

                await _unitOfWork.ResourceRepository.CreateBlog(blog);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.CREATE_FAILED),
                        Messages.Blog.CREATE_FAILED);
                }
                return Result<bool>.Success(true, Messages.Blog.CREATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.CREATE_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> UpdateBlogAsync(Guid id, BlogUpdateDto updateBlogDto, Guid instructorId)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetMyBlogDetailTrackedAsync(id, instructorId);
                if (blog == null)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                var applyError = await ApplyBlogUpdatesAsync(blog, updateBlogDto);
                if (applyError != null)
                {
                    return Result<bool>.Failure(applyError, applyError.Description ?? Messages.Blog.UPDATE_FAILED);
                }

                var updateResult = await _unitOfWork.ResourceRepository.UpdateBlog(blog);
                if (!updateResult)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.UPDATE_FAILED),
                        Messages.Blog.UPDATE_FAILED);
                }

                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.UPDATE_FAILED),
                        Messages.Blog.UPDATE_FAILED);
                }

                return Result<bool>.Success(true, Messages.Blog.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.UPDATE_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        private async Task<ServiceError?> ApplyBlogUpdatesAsync(Blog blog, BlogUpdateDto updateBlogDto)
        {
            var now = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(updateBlogDto.Title))
            {
                blog.Title = updateBlogDto.Title.Trim();
            }

            if (!string.IsNullOrWhiteSpace(updateBlogDto.ThumbnailUrl))
            {
                blog.ThumbnailUrl = updateBlogDto.ThumbnailUrl.Trim();
            }

            if (updateBlogDto.CategoryId.HasValue && updateBlogDto.CategoryId.Value != blog.CategoryId)
            {
                var categoryExists = await _unitOfWork.ResourceRepository.CategoryExistsAsync(updateBlogDto.CategoryId.Value);
                if (!categoryExists)
                {
                    return ServiceError.NotFoundError(Messages.Blog.CATEGORY_NOT_FOUND);
                }

                blog.CategoryId = updateBlogDto.CategoryId.Value;
            }

            if (updateBlogDto.Contents != null && updateBlogDto.Contents.Any())
            {
                blog.Contents ??= new List<BlogContent>();
                var contentLookup = blog.Contents.ToDictionary(c => c.Id);

                foreach (var contentDto in updateBlogDto.Contents)
                {
                    if (contentDto.Id.HasValue && contentLookup.TryGetValue(contentDto.Id.Value, out var existingContent))
                    {
                        if (contentDto.IsDeleted == true)
                        {
                            existingContent.IsDelete = true;
                        }
                        else
                        {
                            if (contentDto.Content != null)
                            {
                                existingContent.Content = contentDto.Content.Trim();
                            }

                            existingContent.IsDelete = false;
                        }

                        existingContent.UpdateAt = now;
                    }
                    else if (!contentDto.Id.HasValue && contentDto.IsDeleted != true)
                    {
                        var newContent = _mapper.Map<BlogContent>(contentDto);
                        newContent.BlogId = blog.Id;
                        newContent.CreatedAt = now;
                        newContent.UpdateAt = now;
                        newContent.IsDelete = false;

                        _unitOfWork.ResourceRepository.AddBlogContent(newContent);
                        blog.Contents.Add(newContent);
                    }
                }
            }

            blog.UpdateAt = now;
            return null;
        }

        public async Task<Result<string>> UploadImageForBlog(IFormFile file)
        {
            try
            {
                string url = _cloudinary.UploadImageFormFileResourceToCloudinary(file, $"blog_{file.Name}_{DateTime.Now}");

                return Result<string>.Success(url);
            }
            catch (Exception exception)
            {
                return Result<string>.Failure(ServiceError.UnhandledException(""),Messages.Commons.UNHANDLED);
            }
        }

        // Inspector APIs
        public async Task<Result<PaginatedList<ResourceDto>>> GetBlogsListAsync(BlogListFilterDTO filter)
        {
            try
            {
                // Convert Status (int?) từ query string sang BlogStatus enum
                BlogStatus? statusEnum = null;
                if (filter.Status.HasValue && Enum.IsDefined(typeof(BlogStatus), filter.Status.Value))
                {
                    statusEnum = (BlogStatus)filter.Status.Value;
                }

                Expression<Func<Blog, bool>> filterExpression = x =>
                    (string.IsNullOrEmpty(filter.SearchKey) || x.Title.Contains(filter.SearchKey)) &&
                    (!filter.CategoryId.HasValue || x.CategoryId == filter.CategoryId.Value) &&
                    (!statusEnum.HasValue || x.Status == statusEnum.Value) &&
                    !x.IsDelete;

                string includedProperties = "Category";

                var allBlogs = await _unitOfWork.ResourceRepository.GetAllBlogsAsync(
                    filter: filterExpression,
                    includeProperties: includedProperties
                );

                var paginatedBlogs = PaginatedList<Blog>.Create(allBlogs, filter.PageNumber, filter.PageSize);

                var blogDtos = _mapper.Map<List<ResourceDto>>(paginatedBlogs.PageContent.ToList());

                var paginatedResult = PaginatedList<ResourceDto>.CreateFromPagedData(
                    blogDtos,
                    paginatedBlogs.CurrentPage,
                    paginatedBlogs.PageSize,
                    paginatedBlogs.TotalCount
                );

                return Result<PaginatedList<ResourceDto>>.Success(paginatedResult, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<PaginatedList<ResourceDto>>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.RETRIEVE_ERROR}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> ApproveBlogAsync(Guid blogId)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(blogId);
                if (blog == null)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                // Chỉ approve được nếu blog đang ở trạng thái Pending hoặc ReApply
                if (blog.Status != BlogStatus.Pending && blog.Status != BlogStatus.ReApply)
                {
                    return Result<bool>.Failure(
                        ServiceError.BadRequestError(Messages.Blog.APPROVE_INVALID_STATUS),
                        Messages.Blog.APPROVE_INVALID_STATUS);
                }

                var updateResult = await _unitOfWork.ResourceRepository.UpdateBlogStatus(blogId, BlogStatus.Active);
                if (!updateResult)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.APPROVE_FAILED),
                        Messages.Blog.APPROVE_FAILED);
                }

                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.APPROVE_FAILED),
                        Messages.Blog.APPROVE_FAILED);
                }

                return Result<bool>.Success(true, Messages.Blog.APPROVE_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.APPROVE_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> RejectBlogAsync(Guid blogId)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(blogId);
                if (blog == null)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                // Chỉ reject được nếu blog đang ở trạng thái Pending hoặc ReApply
                if (blog.Status != BlogStatus.Pending && blog.Status != BlogStatus.ReApply)
                {
                    return Result<bool>.Failure(
                        ServiceError.BadRequestError(Messages.Blog.REJECT_INVALID_STATUS),
                        Messages.Blog.REJECT_INVALID_STATUS);
                }

                var updateResult = await _unitOfWork.ResourceRepository.UpdateBlogStatus(blogId, BlogStatus.Inactive);
                if (!updateResult)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.REJECT_FAILED),
                        Messages.Blog.REJECT_FAILED);
                }

                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.REJECT_FAILED),
                        Messages.Blog.REJECT_FAILED);
                }

                return Result<bool>.Success(true, Messages.Blog.REJECT_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.REJECT_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> BanBlogAsync(Guid blogId)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(blogId);
                if (blog == null)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                var updateResult = await _unitOfWork.ResourceRepository.UpdateBlogStatus(blogId, BlogStatus.Banned);
                if (!updateResult)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.BAN_FAILED),
                        Messages.Blog.BAN_FAILED);
                }

                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0)
                {
                    return Result<bool>.Failure(
                        ServiceError.UnhandledException(Messages.Blog.BAN_FAILED),
                        Messages.Blog.BAN_FAILED);
                }

                return Result<bool>.Success(true, Messages.Blog.BAN_SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.BAN_FAILED}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

    }
}
