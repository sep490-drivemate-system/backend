using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BookingDbContext _context;

        public FeedbackRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.Booking)
                .ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(Guid id)
        {
            return await _context.Feedbacks
                .Include(f => f.Booking)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Feedback>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.Feedbacks
                .Include(f => f.Booking)
                .Where(f => f.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Feedbacks
                .Include(f => f.Booking)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            feedback.Id = Guid.NewGuid();
            feedback.CreatedAt = DateTime.UtcNow;
            feedback.UpdatedAt = DateTime.UtcNow;
            
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            feedback.UpdatedAt = DateTime.UtcNow;
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task DeleteAsync(Guid id)
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
