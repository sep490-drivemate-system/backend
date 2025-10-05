using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;

namespace BookingService.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly BookingDbContext _context;

        public PackageRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            return await _context.Packages
                .Include(p => p.CarPackages)
                .ToListAsync();
        }

        public async Task<Package?> GetByIdAsync(Guid id)
        {
            return await _context.Packages
                .Include(p => p.CarPackages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        //public async Task<IEnumerable<Package>> GetActivePackagesAsync()
        //{
        //    return await _context.Packages
        //        .Include(p => p.CarPackages)
        //        .Where(p => p.IsActive)
        //        .ToListAsync();
        //}

        public async Task<Package> CreateAsync(Package package)
        {
            package.Id = Guid.NewGuid();
            package.CreatedAt = DateTime.UtcNow;
            package.LastModifiedAt = DateTime.UtcNow;
            
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<Package> UpdateAsync(Package package)
        {
            package.LastModifiedAt = DateTime.UtcNow;
            _context.Packages.Update(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task DeleteAsync(Guid id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                await _context.SaveChangesAsync();
            }
        }
    }
}
