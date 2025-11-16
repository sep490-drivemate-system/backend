using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.Persistence.Context;
using MessagingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MessagingService.Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessagingDbContext _context;
        private IConversationRepository? _conversationRepository;
        private IMessageRepository? _messageRepository;
        private INotificationRepository? _notificationRepository;

        public UnitOfWork(MessagingDbContext context)
        {
            _context = context;
        }

        public IConversationRepository ConversationRepository
        {
            get
            {
                _conversationRepository ??= new ConversationRepository(_context);
                return _conversationRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                _messageRepository ??= new MessageRepository(_context);
                return _messageRepository;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                _notificationRepository ??= new NotificationRepository(_context);
                return _notificationRepository;
            }
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
                    case Microsoft.EntityFrameworkCore.EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                        break;

                    case Microsoft.EntityFrameworkCore.EntityState.Added:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        break;

                    case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}

