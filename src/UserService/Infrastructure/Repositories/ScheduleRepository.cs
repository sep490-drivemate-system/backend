using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class ScheduleRepository(ApplicationDbContext context): GenericRepository<ScheduleUnavailability>(context), IScheduleRepository
    {
    }
}
