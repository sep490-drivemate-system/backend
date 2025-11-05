using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Enum;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class PolicyRepository(UserServiceDbContext context): GenericRepository<Policy>(context), IPolicyRepository
    {
    }
}
