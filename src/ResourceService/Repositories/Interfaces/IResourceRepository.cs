using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<List<Blog>> GetBlogsAsync();
        Task<List<Blog>> GetMyBlogsAsync(Guid instructorId);
        Task<(List<Blog> blogs, int totalCount)> GetBlogsPagedAsync(int page, int pageSize);
        Task<Blog?> GetBlogDetailAsync(Guid blogId);
        Task<Blog?> GetMyBlogDetailAsync(Guid id, Guid instructorId);
        Task<bool> SoftDeleteBlogAsync(Guid blogId);

        Task<bool> CreateBlog(Blog blog);
    }
}
