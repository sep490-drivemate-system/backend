using PaymentService.Application.Interfaces;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.Repositories;

namespace PaymentService.Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PaymentDbContext _context;
        private ITransactionRepository _transactionRepository;
        private IWalletRepository _walletRepository;
        
        public UnitOfWork(PaymentDbContext context)
        {
            _context = context;
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransactionRepository(_context);
                }
                return _transactionRepository;
            }
        }

        public IWalletRepository WalletRepository
        {
            get
            {
                if (_walletRepository == null)
                {
                    _walletRepository = new WalletRepository(_context);
                }
                return _walletRepository;
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

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
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
