using CloudinaryDotNet.Core;
using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using ResourceService.Infrastructure.Persistences;

namespace ResourceService.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ResourceDbContext context) : base(context) { }

        public async Task<IEnumerable<Post>> GetForReviewAsync(int page, int pageSize)
        {
            return await _dbSet.AsNoTracking()
                .Where(p => !p.IsDeleted && p.Status == PostStatus.Pending)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Post?> GetWithDetailsAsync(Guid postId)
        {
            return await _dbSet.AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Videos)
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .Include(p => p.Tags)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == postId && !p.IsDeleted);
        }

        public async Task<bool> UpdateStatusAsync(Guid postId, PostStatus status, string? reason, Guid? reviewerId)
        {
            var post = await _dbSet.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null) return false;

            post.Status = status;

            var review = new PostReview
            {
                PostId = postId,
                Reason = reason,
                ReviewerId = reviewerId,
                ReviewedAt = DateTime.Now,
            };

            _context.Set<PostReview>().Add(review);
            return true;
        }
    }
}

