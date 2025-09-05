using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }
    }
}
