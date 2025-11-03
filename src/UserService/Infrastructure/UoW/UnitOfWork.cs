using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.UoW
{
    public class UnitOfWork(UserServiceDbContext context): IUnitOfWork
    {
        private readonly UserServiceDbContext _context = context;
        private readonly Dictionary<Type, object> _repositories = new();

        private IUserRepository _userRepository;
        private IInstructorRepository _instructorRepository;
        private IPolicyRepository _policyRepository;
        private IScheduleRepository _scheduleRepository;
        private INoviceDriverRepository _noviceDriverRepository;
        private IApplicationRepository _applicationRepository;
        private IPolicyRepository _policiesRepository;

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

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public IInstructorRepository InstructorRepository
        {
            get
            {
                if (_instructorRepository == null)
                {
                    _instructorRepository = new InstructorRepository(_context);
                }
                return _instructorRepository;
            }
        }

        public IPolicyRepository PolicyRepository
        {
            get
            {
                if (_policyRepository == null)
                {
                    _policyRepository = new PolicyRepository(_context);
                }
                return _policyRepository;
            }
        }

        public IScheduleRepository ScheduleRepository
        {
            get
            {
                _scheduleRepository ??= new ScheduleRepository(_context);
                return _scheduleRepository;
            }
        }

        public INoviceDriverRepository NoviceDriverRepository
        {
            get
            {
                _noviceDriverRepository ??= new NoviceDriverRepository(_context);
                return _noviceDriverRepository;
            }
        }

        public IPolicyRepository PoliciesRepository
        {
            get
            {
                _policiesRepository ??= new PolicyRepository(_context);
                return _policiesRepository;
            }
        }

        public IApplicationRepository ApplicationRepository
        {
            get
            {
                _applicationRepository ??= new ApplicationRepository(_context);
                return _applicationRepository;
            }
        }
    }
}
