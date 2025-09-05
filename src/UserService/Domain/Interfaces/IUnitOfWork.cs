namespace UserService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task CommitAsync();
        public void RollBack();
    }
}
