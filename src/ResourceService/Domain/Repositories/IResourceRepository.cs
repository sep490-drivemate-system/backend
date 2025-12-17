using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;

namespace ResourceService.Domain.Repositories
{
    public interface IResourceRepository
    {
        Task<List<Blog>> GetBlogsAsync();
        Task<List<Blog>> GetMyBlogsAsync(Guid instructorId);
        Task<List<Blog>> GetAllBlogsAsync(System.Linq.Expressions.Expression<Func<Blog, bool>>? filter = null, string includeProperties = "");
        Task<Blog?> GetBlogDetailAsync(Guid blogId);
        Task<Blog?> GetMyBlogDetailAsync(Guid id, Guid instructorId);
        Task<Blog?> GetMyBlogDetailTrackedAsync(Guid id, Guid instructorId);
        Task<bool> CategoryExistsAsync(Guid categoryId);
        Task<bool> HasBlogsUsingCategoryAsync(Guid categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> CreateCategory(Category category);
        Task<bool> SoftDeleteBlogAsync(Guid blogId);

        Task<bool> CreateBlog(Blog blog);
        Task<bool> UpdateBlog(Blog blog);
        Task<bool> UpdateBlogStatus(Guid blogId, BlogStatus status);
        void AddBlogContent(BlogContent content);
    }
}
