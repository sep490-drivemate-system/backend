using ResourceService.Services.DTOs;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Interfaces
{
    public interface IResourcesService
    {
        Task<Result<PaginatedList<ResourceDto>>> GetBlogsPagedAsync(BlogListFilterBaseDTO filter);
        Task<Result<BlogDetailDto>> GetBlogDetailAsync(Guid id);
        Task<Result<PaginatedList<ResourceDto>>> GetMyBlogsAsync(Guid instructorId, BlogListFilterDTO filter);
        Task<Result<BlogDetailDto>> GetMyBlogDetailAsync( Guid id, Guid instructorId);
        Task<Result<bool>> DeleteBlogAsync(Guid id, Guid instructorId);
        Task<Result<bool>> CreateBlogAsync(BlogCreateDto createBlogDto, Guid instructorId);
        Task<Result<bool>> UpdateBlogAsync(Guid id, BlogUpdateDto updateBlogDto, Guid instructorId);
        Task<Result<ICollection<CategoryDto>>> GetCategoriesAsync();
        
        // Inspector APIs
        Task<Result<PaginatedList<ResourceDto>>> GetPendingBlogsAsync(BlogListFilterDTO filter);
        Task<Result<bool>> ApproveBlogAsync(Guid blogId);
        Task<Result<bool>> RejectBlogAsync(Guid blogId);
        Task<Result<bool>> BanBlogAsync(Guid blogId);
        


        // Upload Image
        Task<Result<string>> UploadImageForBlog(IFormFile file);
    }
}
