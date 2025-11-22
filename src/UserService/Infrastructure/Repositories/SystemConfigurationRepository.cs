using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class SystemConfigurationRepository(UserServiceDbContext dbContext): GenericRepository<SystemConfiguration>(dbContext), ISystemConfigurationRepository
    {

    }
}
