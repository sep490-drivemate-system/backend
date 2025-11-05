using Microsoft.EntityFrameworkCore;
using ResourceService.Repositories.Interfaces;
using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Implementation
{
    public class ResourceRepository: IResourceRepository
    {
        private ResourceDbContext _context;

        public ResourceRepository(ResourceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Blog>> GetBlogsAsync()
        {
            return await _context.Blogs
                .AsNoTracking()
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<(List<Blog> blogs, int totalCount)> GetBlogsPagedAsync(int page, int pageSize)
        {
            var query = _context.Blogs
                .AsNoTracking()
                .Include(b => b.Category);

            var totalCount = await query.CountAsync();

            var blogs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (blogs, totalCount);
        }

        public async Task<Blog?> GetBlogDetailAsync(Guid blogId)
        {
            return await _context.Blogs
                .AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Contents)
                    .ThenInclude(c => c.Images)
                .FirstOrDefaultAsync(b => b.Id == blogId);
        }
    }
}
