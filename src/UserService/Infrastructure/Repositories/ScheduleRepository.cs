using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class ScheduleRepository(UserServiceDbContext context): GenericRepository<PersonalSchedule>(context), IScheduleRepository
    {
    }
}
