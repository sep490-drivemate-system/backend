using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class InstructorRepository(ApplicationDbContext context): GenericRepository<Instructor>(context), IInstructorRepository
    {
        public override async Task<List<Instructor>> GetAllAsync()
        {
            return await _context.Instructors
                .Include(x => x.Cars)
                .Include(x => x.User)
                .Include(x => x.InstructorApplication)
                .ThenInclude(x => x.ApplicationTracking)
                .ToListAsync();
        }

        public override async Task<Instructor?> GetByIdAsync<Tid>(Tid id)
        {
            if (id is Guid guid_id)
            {
                return await _context.Instructors
                .Include(x => x.Cars)
                .Include(x => x.User)
                .Include(x => x.InstructorApplication)
                .ThenInclude(x => x.ApplicationTracking)
                .FirstOrDefaultAsync(x => x.Id == guid_id && !x.IsDelete);
            }

            throw new InvalidDataException("No support for non-guid data as parameter");
        }
    }
}
