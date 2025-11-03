using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class NoviceDriverRepository(UserServiceDbContext context) : GenericRepository<NoviceDriver>(context), INoviceDriverRepository
    {
        public override async Task<NoviceDriver?> GetByIdAsync<Tid>(Tid id)
        {
            if(id is Guid guid_id)
            {
                return await _context.NoviceDrivers.Include(x => x.User).ThenInclude(x => x.Addresses)
                    .FirstOrDefaultAsync(x => x.Id == guid_id && x.IsDelete == false);
            }

            return null;
        }
    }
}
