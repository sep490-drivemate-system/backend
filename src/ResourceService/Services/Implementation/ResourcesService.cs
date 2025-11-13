using AutoMapper;
using System.Linq;
using System.Linq.Expressions;
using ResourceService.Repositories;
using ResourceService.Repositories.Enum;
using ResourceService.Repositories.Interfaces;
using ResourceService.Repositories.Models;
using ResourceService.Services.Commons.Constants;
using ResourceService.Services.DTOs;
using ResourceService.Services.Interfaces;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Implementation
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

        public async Task<Result<ICollection<ResourceDto>>> GetMyBlogsAsync(Guid instructorId)
        {
            try
            {
                var blogs = await _unitOfWork.ResourceRepository.GetMyBlogsAsync(instructorId);
                if (blogs == null)
                {
                    return Result<ICollection<ResourceDto>>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }
                var resourcesService = _mapper.Map<ICollection<ResourceDto>>(blogs);
                return Result<ICollection<ResourceDto>>.Success(resourcesService, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<ICollection<ResourceDto>>.Failure(
                    ServiceError.UnhandledException(Messages.Blog.RETRIEVE_ERROR), 
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<PaginatedList<ResourceDto>>> GetBlogsPagedAsync(BlogListFilterDTO filter)
        {
            try
            {
                Expression<Func<Blog, bool>> filterExpression = x =>
                    (string.IsNullOrEmpty(filter.SearchKey) || x.Title.Contains(filter.SearchKey)) &&
                    (!filter.CategoryId.HasValue || x.CategoryId == filter.CategoryId.Value) &&
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
                return Result<BlogDetailDto>.Success(blogDetail, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<BlogDetailDto>.Failure(
                    ServiceError.UnhandledException($"{Messages.Blog.RETRIEVE_ERROR}: {ex.Message}"),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<BlogDetailDto>> GetMyBlogDetailAsync(Guid instructorId, Guid id)
        {
            try
            {
                var blog = await _unitOfWork.ResourceRepository.GetMyBlogDetailAsync( id, instructorId);
                if (blog == null)
                {
                    return Result<BlogDetailDto>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }
                var blogDetail = _mapper.Map<BlogDetailDto>(blog);
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

                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
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

        public async Task<Result<bool>> CreateBlogAsync(BlogCreateDto createBlogDto, Guid instructorId)
        {
            try
            {
                var blog = _mapper.Map<Blog>(createBlogDto);
                blog.InstructorId = instructorId;
                blog.CreatedAt = DateTime.Now;
                blog.Status = BlogStatus.Active;
                blog.IsDelete = false;

                await _unitOfWork.ResourceRepository.CreateBlog(blog);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
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

                var saveResult = await _unitOfWork.SaveChangesWithTransactionAsync();
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

                            if (contentDto.No.HasValue)
                            {
                                existingContent.No = contentDto.No.Value;
                            }

                            if (contentDto.ImageUrl != null)
                            {
                                existingContent.ImageUrl = contentDto.ImageUrl;
                            }

                            existingContent.IsDelete = false;
                        }

                        existingContent.UpdateAt = now;
                    }
                    else if (!contentDto.Id.HasValue && contentDto.IsDeleted != true)
                    {
                        var newContent = new BlogContent
                        {
                            BlogId = blog.Id,
                            Content = contentDto.Content?.Trim() ?? string.Empty,
                            No = contentDto.No ?? 0,
                            ImageUrl = contentDto.ImageUrl ?? string.Empty,
                            CreatedAt = now,
                            UpdateAt = now,
                            IsDelete = false
                        };

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
    }
}
