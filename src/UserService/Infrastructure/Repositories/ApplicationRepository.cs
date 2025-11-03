using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class ApplicationRepository(UserServiceDbContext context): GenericRepository<InstructorApplication>(context), IApplicationRepository
    {
    }
}
