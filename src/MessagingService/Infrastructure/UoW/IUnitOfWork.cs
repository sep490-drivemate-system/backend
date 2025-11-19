using MessagingService.Domain.Interfaces;

namespace MessagingService.Infrastructure.UoW
{
    public interface IUnitOfWork
    {
        IConversationRepository ConversationRepository { get; }
        IMessageRepository MessageRepository { get; }
        INotificationRepository NotificationRepository { get; }
        Task<int> CommitChangesAsync();
        void RevertChanges();
    }
}

