using UserService.Domain.Interfaces;

namespace UserService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository{ get; }
        public void Commit();
        public Task CommitAsync();
        public void RollBack();
    }
}
