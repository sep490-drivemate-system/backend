using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Domain.Enum;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class DrivingSessionRepository : GenericRepository<DrivingSession>, IDrivingSessionRepository
    {
        public DrivingSessionRepository(BookingDbContext context) : base(context) { }

        public async Task<IEnumerable<DrivingSession>> GetAllByStatus(SessionStatus status)
        {
            return await _context.DrivingSessions
                .Where(ds => ds.Status == status && !ds.IsDeleted)
                .Include(ds => ds.Bookings)
                .Include(ds => ds.SessionRoadTypes)
                .Include(ds => ds.SessionLogs)
                .Include(ds => ds.SessionRoutes)
                .OrderByDescending(ds => ds.CreatedAt)
                .ToListAsync();
        }
    }
}