using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(Guid id);
        //Task<IEnumerable<Package>> GetActivePackagesAsync();
        Task<Package> CreatePackageAsync(Package package);
        Task<Package> UpdatePackageAsync(Package package);
        Task DeletePackageAsync(Guid id);
        Task<List<Package>> GetInstructorPackages(Guid instructorId);
    }
}
