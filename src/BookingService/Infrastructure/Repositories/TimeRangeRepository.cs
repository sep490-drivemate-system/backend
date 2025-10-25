using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class TimeRangeRepository : GenericRepository<TimeRange>, ITimeRangeRepository
    {
        public TimeRangeRepository(BookingDbContext context) : base(context) { }



    }
}
