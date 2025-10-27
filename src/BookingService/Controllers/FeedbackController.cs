using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public FeedbackController(BookingDbContext context)
        {
            _context = context;
        }

        [HttpPost("list-instructor")]
        public async Task<ActionResult<FeedbackResponse>> GetStatisticFeedback([FromBody] FeedbackRequest request)
        {
            try
            {
                if (request?.ListInstructor == null || !request.ListInstructor.Any())
                {
                    return BadRequest("ListInstructor cannot be null or empty");
                }

                // Since we don't have direct InstructorId in current schema,
                // we'll return aggregated statistics for all requested instructors
                // In a real scenario, you would need to add InstructorId to Booking or DrivingSession
                
                // Get all bookings and their feedbacks
                var allBookings = await _context.Bookings
                    .Where(b => !b.IsDeleted)
                    .Include(b => b.Feedbacks)
                    .Include(b => b.CarPackages)
                    .ThenInclude(cp => cp.Packages)
                    .ToListAsync();

                // Calculate aggregated statistics
                var totalBookings = allBookings.Count();
                
                var allFeedbacks = allBookings
                    .SelectMany(b => b.Feedbacks)
                    .Where(f => !f.IsDeleted)
                    .ToList();

                var averageRating = allFeedbacks.Any() 
                    ? allFeedbacks.Average(f => f.InstructorRating) 
                    : 0.0;

                var averageUnitPrice = allBookings
                    .Where(b => b.CarPackages?.Packages != null)
                    .Select(b => b.CarPackages.Packages.RecommendedValue)
                    .DefaultIfEmpty(0)
                    .Average();

                // Return aggregated data for the first instructor (or modify logic as needed)
                var response = new FeedbackResponse
                {
                    Id = request.ListInstructor.FirstOrDefault(),
                    BookingCount = totalBookings,
                    AverageRating = averageRating,
                    UnitPrice = averageUnitPrice
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
