using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IPackageRepository
    {
        Task<IEnumerable<Package>> GetAllAsync();
        Task<Package?> GetByIdAsync(Guid id);
        //Task<IEnumerable<Package>> GetActivePackagesAsync();
        Task<Package> CreateAsync(Package package);
        Task<Package> UpdateAsync(Package package);
        Task DeleteAsync(Guid id);
    }
}
