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
            _context = context;
        }

        public ResourceRepository() => _context ??= new ResourceDbContext();
    }
}
