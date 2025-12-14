using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using ResourceService.Infrastructure.Persistences;

namespace ResourceService.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ResourceDbContext context) : base(context) { }
    }
}

