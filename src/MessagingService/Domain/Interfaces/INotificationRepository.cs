using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;

namespace MessagingService.Domain.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, int pageNumber = 1, int pageSize = 10);
        Task<int> GetUnreadNotificationCountAsync(Guid userId);
        Task MarkNotificationAsReadAsync(Guid notificationId);
        Task MarkAllNotificationsAsReadAsync(Guid userId);
    }
}

