using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class CarRepository(ApplicationDbContext context): GenericRepository<Car>(context), ICarRepository
    {
        public override async Task<Car?> GetByIdAsync<Tid>(Tid id)
        {
            if (id is Guid guid_id)
            {
                var car = await _context.Cars.Include(x => x.Manufacturer)
                .Include(x => x.CarImages)
                .Include(x => x.LicenseCategory)
                .Include(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == guid_id);

                return car;
            }

            throw new InvalidDataException("No support for non-guid data as parameter");
        }

        public override async Task<List<Car>> GetAllAsync()
        {
            var car = await _context.Cars.Include(x => x.Manufacturer)
                .Include(x => x.CarImages)
                .Include(x => x.LicenseCategory)
                .Include(x => x.Instructor).ToListAsync();

            return car;
        }
    }
}
