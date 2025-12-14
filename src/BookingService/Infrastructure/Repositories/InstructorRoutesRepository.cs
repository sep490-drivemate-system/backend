using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;

namespace BookingService.Infrastructure.Repositories
{
    public class InstructorRoutesRepository : GenericRepository<InstructorRoutes>, IInstructorRoutesRepository
    {
        public InstructorRoutesRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InstructorRoutes>> GetByInstructorAsync(Guid instructorId)
        {
            return await _context.Set<InstructorRoutes>()
                .Where(r => r.InstructorId == instructorId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
