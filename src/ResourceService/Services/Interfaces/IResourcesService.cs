using ResourceService.Services.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Interfaces
{
    public interface IResourcesService
    {
        Task<Result<ICollection<ResourceDto>>> GetBlogsAsync();
        Task<Result<PagedResult<ResourceDto>>> GetBlogsPagedAsync(int page, int pageSize);
        Task<Result<BlogDetailDto>> GetBlogDetailAsync(Guid id);
        Task<Result<ICollection<ResourceDto>>> GetMyBlogsAsync(Guid instructorId);
        Task<Result<BlogDetailDto>> GetMyBlogDetailAsync( Guid id, Guid instructorId);
        Task<Result<bool>> DeleteBlogAsync(Guid id, Guid instructorId);
        Task<Result<bool>> CreateBlogAsync(BlogCreateDto createBlogDto, Guid instructorId);
        Task<Result<bool>> UpdateBlogAsync(Guid id, BlogUpdateDto updateBlogDto, Guid instructorId);
        Task<Result<ICollection<CategoryDto>>> GetCategoriesAsync();
        


        // Upload Image
        Task<Result<string>> UploadImageForBlog(IFormFile file);
    }
}
