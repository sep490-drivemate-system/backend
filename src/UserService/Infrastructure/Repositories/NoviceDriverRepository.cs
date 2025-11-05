using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class NoviceDriverRepository(UserServiceDbContext context) : GenericRepository<NoviceDriver>(context), INoviceDriverRepository
    {

    }
}
