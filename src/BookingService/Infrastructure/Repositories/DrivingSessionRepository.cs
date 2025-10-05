using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class DrivingSessionRepository : IDrivingSessionRepository
    {
        private readonly BookingDbContext _context;

        public DrivingSessionRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DrivingSession>> GetAllAsync()
        {
            return await _context.DrivingSessions
                .Include(ds => ds.Bookings)
                .Include(ds => ds.SessionRoutes)
                .Include(sr => sr.SessionLogs)
                .ToListAsync();
        }

        public async Task<DrivingSession?> GetByIdAsync(Guid id)
        {
            return await _context.DrivingSessions
                .Include(ds => ds.Bookings)
                .Include(ds => ds.SessionRoutes)
                .Include(sr => sr.SessionLogs)
                .FirstOrDefaultAsync(ds => ds.Id == id);
        }

        public async Task<IEnumerable<DrivingSession>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.DrivingSessions
                .Include(ds => ds.Bookings)
                .Include(ds => ds.SessionRoutes)
                .Include(sr => sr.SessionLogs)
                .Where(ds => ds.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<DrivingSession> CreateAsync(DrivingSession session)
        {
            session.Id = Guid.NewGuid();
            session.CreatedAt = DateTime.UtcNow;
            session.LastModifiedAt = DateTime.UtcNow;
            
            _context.DrivingSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<DrivingSession> UpdateAsync(DrivingSession session)
        {
            session.LastModifiedAt = DateTime.UtcNow;
            _context.DrivingSessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task DeleteAsync(Guid id)
        {
            var session = await _context.DrivingSessions.FindAsync(id);
            if (session != null)
            {
                _context.DrivingSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }
}
