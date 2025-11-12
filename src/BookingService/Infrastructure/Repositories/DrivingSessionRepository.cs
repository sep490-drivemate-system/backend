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
                .Include(ds => ds.Booking)
                .Include(ds => ds.SessionLogs)
                .Include(ds => ds.SessionRoutes)
                .OrderByDescending(ds => ds.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DrivingSession>> GetSessionsByInstructorIdAsync(Guid instructorId, SessionStatus? status = null)
        {
            var query = _context.DrivingSessions
                .Include(ds => ds.Booking)
                    .ThenInclude(b => b.Package)
                .Include(ds => ds.Booking)
                    .ThenInclude(b => b.Car)
                .Include(ds => ds.SessionRoutes)
                .Where(ds => ds.Booking.InstructorId == instructorId && !ds.IsDeleted && !ds.Booking.IsDeleted);

            if (status.HasValue && status.Value != 0)
            {
                query = query.Where(ds => ds.Status == status.Value);
            }

            return await query
                .OrderByDescending(ds => ds.CreatedAt)
                .ToListAsync();
        }
    }
}