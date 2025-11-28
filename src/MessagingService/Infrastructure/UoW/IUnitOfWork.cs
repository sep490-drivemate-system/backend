using MessagingService.Domain.Interfaces;

namespace MessagingService.Infrastructure.UoW
{
    public interface IUnitOfWork
    {
        IChatSessionRepository ChatSessionRepository { get; }
        IMessageRepository MessageRepository { get; }
        INotificationRepository NotificationRepository { get; }
        Task<int> CommitChangesAsync();
        void RevertChanges();
    }
}

