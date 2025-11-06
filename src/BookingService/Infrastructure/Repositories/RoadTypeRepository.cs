using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories
{
    public class RoadTypeRepository : GenericRepository<RoadType>, IRoadTypeRepository
    {
        public RoadTypeRepository(BookingDbContext context) : base(context)
        {
        }
    }
}
