using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class PolicyRepository(ApplicationDbContext context): GenericRepository<Policy>(context), IPolicyRepository
    {

    }
}
