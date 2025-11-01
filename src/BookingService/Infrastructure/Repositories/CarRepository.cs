using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Repositories
{
    public class CarRepository(BookingDbContext context): GenericRepository<Car>(context), ICarRepository
    {
        public async Task<ICollection<CarImage>?> GetCarImages(Guid car_id)
        {
            var car = await _dbSet.Include(x => x.CarImages).SingleOrDefaultAsync(x => x.Id == car_id);

            if (car == null)
            {
                return null;
            }

            return car.CarImages;
        }
    }
}
