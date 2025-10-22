using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.Bookings)
                .ToListAsync();
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(Guid id)
        {
            return await _context.Feedbacks
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Feedback>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.Feedbacks
                .Include(f => f.Bookings)
                .Where(f => f.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Feedbacks
                .Include(f => f.Bookings)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            feedback.Id = Guid.NewGuid();
            feedback.CreatedAt = DateTime.UtcNow;
            feedback.LastModifiedAt = DateTime.UtcNow;
            
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
        {
            feedback.LastModifiedAt = DateTime.UtcNow;
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task DeleteFeedbackAsync(Guid id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }
    }
}
