using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context ;
        private IUserRepository _userRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IInstructorRepository _instructorRepository;
        private ICarRepository _carRepository;
        private IPolicyRepository _policyRepository;
        private IScheduleRepository _scheduleRepository;
        private INoviceDriverRepository _noviceDriverRepository;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
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
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                if (_refreshTokenRepository == null)
                {
                    _refreshTokenRepository = new RefreshTokenRepository(_context);
                }
                return _refreshTokenRepository;
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

        public ICarRepository CarRepository
        {
            get
            {
                if (_carRepository == null)
                {
                    _carRepository = new CarRepository(_context);
                }
                return _carRepository;
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

        public void Commit()
        {
         _context.SaveChanges();  
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void RollBack()
        {
            _context.ChangeTracker.Clear();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
