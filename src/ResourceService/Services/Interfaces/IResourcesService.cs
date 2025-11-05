using ResourceService.Services.DTOs;

namespace ResourceService.Services.Interfaces
{
    public interface IResourcesService
    {
        Task<IEnumerable<ResourceDto>> GetBlogsAsync();
        Task<PagedResult<ResourceDto>> GetBlogsPagedAsync(int page, int pageSize);
        Task<BlogDetailDto?> GetBlogDetailAsync(Guid id);
        Task<bool> DeleteBlogAsync(Guid id);
    }
}
