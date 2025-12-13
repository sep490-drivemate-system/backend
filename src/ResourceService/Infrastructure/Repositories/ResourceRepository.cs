using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using System.Linq.Expressions;

namespace ResourceService.Infrastructure.Repositories
{
    public class ResourceRepository : GenericRepository<Blog>, IResourceRepository
    {

        public ResourceRepository(DbContext context) : base(context) {}

        public async Task<List<Blog>> GetBlogsAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(b => b.Category)
                .ToListAsync();
        }
        public async Task<List<Blog>> GetMyBlogsAsync(Guid instructorId)
        {
            return await _dbSet.AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetAllBlogsAsync(Expression<Func<Blog, bool>>? filter = null, string includeProperties = "")
        {
            // Đơn giản: chỉ gọi GetAllAsync từ GenericRepository
            // Filter đã được combine sẵn ở service layer (giống GetInstructors)
            return await GetAllAsync(filter: filter, include_properties: includeProperties, disable_tracking: true);
        }

        public async Task<Blog?> GetBlogDetailAsync(Guid blogId)
        {
            return await _dbSet.AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Contents)
                .FirstOrDefaultAsync(b => b.Id == blogId);
        }

        public async Task<Blog?> GetMyBlogDetailAsync(Guid id, Guid instructorId)
        {
            return await _dbSet.AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Contents)
                .FirstOrDefaultAsync(b => b.Id == id && b.InstructorId == instructorId && !b.IsDelete);
        }

        public async Task<Blog?> GetMyBlogDetailTrackedAsync(Guid id, Guid instructorId)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Include(b => b.Contents)
                .FirstOrDefaultAsync(b => b.Id == id && b.InstructorId == instructorId);
        }

        public async Task<bool> CategoryExistsAsync(Guid categoryId)
        {
            return await _context.Set<Category>().AnyAsync(c => c.Id == categoryId && !c.IsDelete);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Set<Category>().AsNoTracking()
                .Where(c => !c.IsDelete)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await _context.Set<Category>().AddAsync(category);
            return true;
        }

        public void AddBlogContent(BlogContent content)
        {
            _context.Set<BlogContent>().Add(content);
        }

        public async Task<bool> SoftDeleteBlogAsync(Guid blogId)
        {
            var blog = await _dbSet.IgnoreQueryFilters() 
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
            await _dbSet.AddAsync(blog);
            return true;
        }

        public Task<bool> UpdateBlog(Blog blog)
        {
            var entry = _context.Entry(blog);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(blog);
            }

            return Task.FromResult(true);
        }

        public async Task<bool> UpdateBlogStatus(Guid blogId, BlogStatus status)
        {
            var blog = await _dbSet.FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null) return false;

            blog.Status = status;
            blog.UpdateAt = DateTime.Now;

            return true;
        }
    }
    
}
