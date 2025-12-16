using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Commons.DTOs.Category;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Interfaces.Services
{
    public interface IResourcesService
    {
        Task<Result<PaginatedList<ResourceDto>>> GetBlogsPagedAsync(BlogListFilterBaseDTO filter);
        Task<Result<BlogDetailDto>> GetBlogDetailAsync(Guid id);
        Task<Result<PaginatedList<ResourceDto>>> GetMyBlogsAsync(Guid instructorId, BlogListFilterDTO filter);
        Task<Result<BlogDetailDto>> GetMyBlogDetailAsync( Guid id, Guid instructorId);
        Task<Result<bool>> DeleteBlogAsync(Guid id, Guid instructorId);
        Task<Result<bool>> CreateBlogAsync(BlogCreateRequest request, Guid instructorId);
        Task<Result<bool>> UpdateBlogAsync(Guid id, BlogUpdateRequest request, Guid instructorId);
        Task<Result<ICollection<CategoryDto>>> GetCategoriesAsync();
        Task<Result<ICollection<BlogStatusDto>>> GetBlogStatusesAsync();
        
        // Inspector APIs
        Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<Result<PaginatedList<ResourceDto>>> GetBlogsListAsync(BlogListFilterDTO filter);
        Task<Result<bool>> ApproveBlogAsync(Guid blogId);
        Task<Result<bool>> RejectBlogAsync(Guid blogId);
        Task<Result<bool>> BanBlogAsync(Guid blogId);
        Task<Result<bool>> UnbanBlogAsync(Guid blogId);
        


        // Upload Image
        Task<Result<string>> UploadImageForBlog(IFormFile file);
    }
}
