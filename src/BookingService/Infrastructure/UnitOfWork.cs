using BookingService.Application.Interfaces;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Infrastructure.Repositories;

namespace BookingService.Infrastructure
{
    public class UnitOfWork(BookingDbContext context): IUnitOfWork
    {
        private readonly BookingDbContext _context = context;

        private IDrivingSkillRepository _skillRepo;

        private IRoadTypeRepository _roadRepo;

        public async Task<int> CommitChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public int ReverChanges()
        {
            throw new NotImplementedException();
        }

        public IRoadTypeRepository RoadTypeRepository
        {
            get
            {
                _roadRepo ??= new RoadTypeRepository(_context);
                return _roadRepo;
            }
        }

        public IDrivingSkillRepository SkillRepository
        {
            get
            {
                _skillRepo ??= new DrivingSkillRepository(_context);
                return _skillRepo;
            }
        }
    }
}
