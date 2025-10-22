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

        public async Task<RoadType> CreateRoadType(RoadType info)
        {
            await _context.RoadTypes.AddAsync(info);
            return info;
        }

        public async Task<List<RoadType>> GetAllRoadType()
        {
            return await _context.RoadTypes.ToListAsync();
        }

        public async Task<RoadType?> GetRoadTypeById(Guid id)
        {
            return await _context.RoadTypes.FindAsync(id);
        }

        public Task<RoadType> RemoveRoadType(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RoadType> UpdateRoadType(RoadType info)
        {
            throw new NotImplementedException();
        }
    }
}
