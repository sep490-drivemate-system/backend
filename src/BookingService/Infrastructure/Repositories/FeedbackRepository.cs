using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;

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
                .Include(f => f.Booking)
                .ToListAsync();
        }

        public async Task<InstructorOverviewFeedbackResponse> GetStatisticListInstructor(Guid instructorId)
        {
                var instructorFeedbacks = await _context.Feedbacks
                    .Where(f => f.InstructorId == instructorId && !f.IsDeleted)
                    .ToListAsync();

                var bookingCount = instructorFeedbacks
                    .Select(f => f.BookingId)
                    .Distinct()
                    .Count();

                var averageRating = instructorFeedbacks.Any() 
                    ? (decimal)instructorFeedbacks.Average(f => f.InstructorRating)
                    : 0m;
            var instructorPackage= await _context.Packages
                   .Where(f => f.InstructorId == instructorId && !f.IsDeleted)
                   .ToListAsync();

            var instructorPackageCount = instructorPackage
                  .Select(f => f.InstructorId)
                    .Count();


            var instructorOverviewStatistics = new InstructorOverviewFeedbackResponse
            {
                AverageRating = averageRating,
                BookingCount = bookingCount,
                PackageCount = instructorPackageCount
            };

            return instructorOverviewStatistics;

        }

        public async Task<Feedback?> GetFeedbackByIdAsync(Guid id)
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
                .Where(f => f.NoviceDriverId == userId)
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

        public async Task<Dictionary<Guid, InstructorOverviewFeedbackResponse>> GetBatchStatistics(List<Guid> instructorIds)
        {
            if (instructorIds == null || !instructorIds.Any())
            {
                return new Dictionary<Guid, InstructorOverviewFeedbackResponse>();
            }

            var feedbackStats = await _context.Feedbacks
                .Where(f => instructorIds.Contains(f.InstructorId) && !f.IsDeleted)
                .GroupBy(f => f.InstructorId)
                .Select(g => new
                {
                    InstructorId = g.Key,
                    AverageRating = g.Average(f => (decimal)f.InstructorRating),
                })
                .ToListAsync();

            var packageStats = await _context.Packages
                .Where(p => instructorIds.Contains(p.InstructorId) && !p.IsDeleted)
                .GroupBy(p => p.InstructorId)
                .Select(g => new
                {
                    InstructorId = g.Key,
                    PackageCount = g.Count()
                })
                .ToListAsync();
            var bookingStats = await _context.Bookings
              .Where(p => instructorIds.Contains(p.InstructorId) && !p.IsDeleted)
              .GroupBy(p => p.InstructorId)
              .Select(g => new
              {
                  InstructorId = g.Key,
                  BookingCount = g.Select(f => f.InstructorId).Distinct().Count()
              })
              .ToListAsync();
            var result = new Dictionary<Guid, InstructorOverviewFeedbackResponse>();

            foreach (var instructorId in instructorIds)
            {
                var feedback = feedbackStats.FirstOrDefault(f => f.InstructorId == instructorId);
                var package = packageStats.FirstOrDefault(p => p.InstructorId == instructorId);
                var booking = bookingStats.FirstOrDefault(p => p.InstructorId == instructorId);

                result[instructorId] = new InstructorOverviewFeedbackResponse
                {
                    AverageRating = feedback?.AverageRating ?? 0m,
                    BookingCount = booking?.BookingCount ?? 0,
                    PackageCount = package?.PackageCount ?? 0
                };
            }

            return result;
        }
    }
}
