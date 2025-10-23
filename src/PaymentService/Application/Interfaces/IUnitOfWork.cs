using PaymentService.Domain.Interfaces;

namespace PaymentService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public ITransactionRepository TransactionRepository { get; }
        public IWalletRepository WalletRepository { get; }

        public void Commit();
        public Task CommitAsync();
        public void RollBack();
    }
}
