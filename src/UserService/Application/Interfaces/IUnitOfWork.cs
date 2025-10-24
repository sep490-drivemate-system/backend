using UserService.Domain.Interfaces;

namespace UserService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository{ get; }
        public IInstructorRepository InstructorRepository { get; }
        public IScheduleRepository ScheduleRepository { get; }
        public ICarRepository CarRepository { get; }
        public IPolicyRepository PolicyRepository { get; }
        public INoviceDriverRepository NoviceDriverRepository { get; }

        public void Commit();
        public Task CommitAsync();
        public void RollBack();
    }
}
