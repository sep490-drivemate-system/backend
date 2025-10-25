using BookingService.Application.Interfaces;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Infrastructure.Repositories;

namespace BookingService.Infrastructure.UoW
{
    public class UnitOfWork(BookingDbContext context): IUnitOfWork
    {
        private readonly BookingDbContext _context = context;

        private ITimeRangeRepository _timeRangeRepo;
        private IDrivingSkillRepository _skillRepo;
        private IRoadTypeRepository _roadRepo;
        private IDrivingSessionRepository _sessionRepo;
        private IBookingRepository _bookRepository;
        private IPackageRepository _packageRepo;
        private IFeedbackRepository _feedbackRepo;

        public async Task<int> CommitChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public int ReverChanges()
        {
            throw new NotImplementedException();
        }

        public IDrivingSessionRepository DrivingSessionRepository
        {
            get
            {
                _sessionRepo ??= new DrivingSessionRepository(_context);
                return _sessionRepo;
            }
        }
        public ITimeRangeRepository TimeRangeRepository
        {
            get
            {
                _timeRangeRepo ??= new TimeRangeRepository(_context);
                return _timeRangeRepo;
            }
        }
        public IBookingRepository BookingRepository
        {
            get
            {
                _bookRepository ??= new BookingRepository(_context);
                return _bookRepository;
            }
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

        public IPackageRepository PackageRepository
        {
            get
            {
                _packageRepo ??= new PackageRepository(_context);
                return _packageRepo;
            }
        }

        public IFeedbackRepository FeedbackRepository
        {
            get
            {
                _feedbackRepo ??= new FeedbackRepository(_context);
                return _feedbackRepo;
            }
        }
    }
}
