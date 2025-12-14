using BookingService.Application.Interfaces;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserService.Infrastructure.Repositories;

namespace BookingService.Infrastructure.UoW
{
    public class UnitOfWork(BookingDbContext context) : IUnitOfWork
    {
        private readonly BookingDbContext _context = context;
        private readonly Dictionary<Type, object> _repositories = new();

        private ICarRepository _carRepo;
        private IDrivingSkillRepository _skillRepo;
        private IRoadTypeRepository _roadRepo;
        private IDrivingSessionRepository _sessionRepo;
        private IBookingRepository _bookRepository;
        private IPackageRepository _packageRepo;
        private IFeedbackRepository _feedbackRepo;
        private ISessionRouteRepository _sessionRouteRepo;
        private ISessionLogRepository _sessionLogRepo;
        private IManufacturerRepository _manufacturerRepo;
        private IInstructorRoutesRepository _instructorRoutesRepo;

        public IGenericRepository<IEntity> Repository<IEntity>() where IEntity : class
        {
            var type = typeof(IEntity);

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new GenericRepository<IEntity>(_context);
                _repositories[type] = repoInstance;
            }

            return (IGenericRepository<IEntity>)_repositories[type];

        }

        public async Task<int> CommitChangesAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void RevertChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public EntityEntry GetTrackingEntry(object entity)
        {
            return _context.Entry(entity);
        }

        public ICarRepository CarRepository
        {
            get
            {
                _carRepo ??= new CarRepository(_context);
                return _carRepo;
            }
        }

        public IDrivingSessionRepository DrivingSessionRepository
        {
            get
            {
                _sessionRepo ??= new DrivingSessionRepository(_context);
                return _sessionRepo;
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

        public IInstructorRoutesRepository InstructorRoutesRepository
        {
            get
            {
                _instructorRoutesRepo ??= new InstructorRoutesRepository(_context);
                return _instructorRoutesRepo;
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

        public ISessionRouteRepository SessionRouteRepository
        {
            get
            {
                _sessionRouteRepo ??= new SessionRouteRepository(_context);
                return _sessionRouteRepo;
            }
        }

        public ISessionLogRepository SessionLogRepository
        {
            get
            {
                _sessionLogRepo ??= new SessionLogRepository(_context);
                return _sessionLogRepo;
            }
        }

        public IManufacturerRepository ManufacturerRepository
        {
            get
            {
                return _manufacturerRepo ??= new ManufacturerRepository(_context);
            }
        }
    }
}
