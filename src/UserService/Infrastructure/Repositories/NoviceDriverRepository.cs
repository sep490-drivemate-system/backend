using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class NoviceDriverRepository(UserServiceDbContext context) : GenericRepository<NoviceDriver>(context), INoviceDriverRepository
    {
        public async Task<NoviceDriver?> GetByIdWithUserAsync(Guid id)
        {
            return await _context.NoviceDrivers
                .Include(nd => nd.User)
                .FirstOrDefaultAsync(nd => nd.Id == id && !nd.IsDeleted);
        }
    }
}
