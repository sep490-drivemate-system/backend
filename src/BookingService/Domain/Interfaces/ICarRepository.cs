using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface ICarRepository: IGenericRepository<Car>
    {
        Task<ICollection<CarImage>?> GetCarImages(Guid car_id);
    }
}
