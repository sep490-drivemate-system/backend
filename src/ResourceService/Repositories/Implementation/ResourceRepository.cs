using Microsoft.EntityFrameworkCore;
using ResourceService.Repositories.Interfaces;
using ResourceService.Repositories.Models;
using System.Linq;

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
        public async Task<List<Blog>> GetMyBlogsAsync(Guid instructorId)
        {
            return await _context.Blogs
                .AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.InstructorId == instructorId)
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
                .FirstOrDefaultAsync(b => b.Id == blogId);
        }

        public async Task<Blog?> GetMyBlogDetailAsync(Guid id, Guid instructorId)
        {
            return await _context.Blogs
                .AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Contents)
                .FirstOrDefaultAsync(b => b.Id == id && b.InstructorId == instructorId);
        }

        public async Task<Blog?> GetMyBlogDetailTrackedAsync(Guid id, Guid instructorId)
        {
            return await _context.Blogs
                .Include(b => b.Category)
                .Include(b => b.Contents)
                .FirstOrDefaultAsync(b => b.Id == id && b.InstructorId == instructorId);
        }

        public async Task<bool> CategoryExistsAsync(Guid categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId && !c.IsDelete);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => !c.IsDelete)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public void AddBlogContent(BlogContent content)
        {
            _context.BlogContents.Add(content);
        }

        public async Task<bool> SoftDeleteBlogAsync(Guid blogId)
        {
            var blog = await _context.Blogs
                .IgnoreQueryFilters() 
                .Include(b => b.Category)
                .Include(b => b.Contents)
                .FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null) return false;

            blog.IsDelete = true;
            blog.UpdateAt = DateTime.Now;

            if (blog.Contents != null)
            {
                foreach (var content in blog.Contents)
                {
                    content.IsDelete = true;
                    content.UpdateAt = DateTime.Now;
                }
            }

            return true;
        }
        public async Task<bool> CreateBlog(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            return true;
        }

        public Task<bool> UpdateBlog(Blog blog)
        {
            var entry = _context.Entry(blog);
            if (entry.State == EntityState.Detached)
            {
                _context.Blogs.Attach(blog);
            }
            return Task.FromResult(true);
        }
    }
    
}
