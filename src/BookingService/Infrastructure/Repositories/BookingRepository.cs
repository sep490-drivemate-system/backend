using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Domain.Enum;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingDbContext context) : base(context) { }

        public async Task<List<Booking>> GetBookingsByDriverIdAsync(Guid driverId, BookingStatus? status = null)
        {
            IQueryable<Booking> query = _context.Set<Booking>()
                .Include(b => b.Package)
                .Include(b => b.DrivingSessions)
                .Include(b => b.Car);

            // Filter by driver ID and not deleted
            query = query.Where(b => b.DriverId == driverId && !b.IsDeleted);

            // Filter by status if provided (status = 0 or null means get all statuses)
            if (status.HasValue && status.Value != 0)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            return await query.ToListAsync();
        }
    }
}
