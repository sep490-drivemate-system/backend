using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using ResourceService.Infrastructure.Persistences;

namespace ResourceService.Infrastructure.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ResourceDbContext context) : base(context) { }
    }
}

