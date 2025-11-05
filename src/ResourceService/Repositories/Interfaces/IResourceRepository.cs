using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<List<Blog>> GetBlogsAsync();
        Task<(List<Blog> blogs, int totalCount)> GetBlogsPagedAsync(int page, int pageSize);
        Task<Blog?> GetBlogDetailAsync(Guid blogId);
        Task<bool> SoftDeleteBlogAsync(Guid blogId);
    }
}
