using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;

namespace ResourceService.Domain.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<IEnumerable<Post>> GetForReviewAsync(int page, int pageSize);
        Task<Post?> GetWithDetailsAsync(Guid postId);
        Task<bool> UpdateStatusAsync(Guid postId, PostStatus status, string? reason, Guid? reviewerId);
    }
}

