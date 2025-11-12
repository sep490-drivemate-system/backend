using AutoMapper;
using ResourceService.Repositories;
using ResourceService.Repositories.Enum;
using ResourceService.Repositories.Interfaces;
using ResourceService.Repositories.Models;
using ResourceService.Services.Commons.Constants;
using ResourceService.Services.DTOs;
using ResourceService.Services.Interfaces;
using SharedLibrary.CloudinaryStorage;
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

        public async Task<Result<ICollection<ResourceDto>>> GetBlogsAsync()
        {
            try
            {
                var blogs = await _unitOfWork.ResourceRepository.GetBlogsAsync();

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

        public async Task<Result<PagedResult<ResourceDto>>> GetBlogsPagedAsync(int page, int pageSize)
        {
            try
            {
                var (blogs, totalCount) = await _unitOfWork.ResourceRepository.GetBlogsPagedAsync(page, pageSize);

                if (blogs == null || blogs.Count == 0)
                {
                    return Result<PagedResult<ResourceDto>>.Failure(
                        ServiceError.NotFoundError(Messages.Blog.NOTFOUND),
                        Messages.Blog.NOTFOUND);
                }

                var blogDtos = _mapper.Map<IEnumerable<ResourceDto>>(blogs);

                var pagedResult = new PagedResult<ResourceDto>
                {
                    Data = blogDtos,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };

                return Result<PagedResult<ResourceDto>>.Success(pagedResult, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ResourceDto>>.Failure(
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
        
        //public async Task<Result<bool>> UpdateBlogAsync(Guid id, BlogUpdateDto updateBlogDto, Guid instructorId)
        //{
        //    try
        //    {
        //        var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<bool>.Failure(
        //            ServiceError.UnhandledException($"{Messages.Blog.UPDATE_FAILED}: {ex.Message}"),
        //            Messages.Commons.UNHANDLED);
        //    }
        //}

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
