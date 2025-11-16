using MessagingService.Domain.Entities;
using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.Persistence.Context;
using MessagingService.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(MessagingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, int pageNumber = 1, int pageSize = 50)
        {
            return await _dbSet
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetUnreadNotificationCountAsync(Guid userId)
        {
            return await _dbSet
                .CountAsync(n => n.UserId == userId && 
                               n.Status == NotificationStatus.Unread && 
                               !n.IsDeleted);
        }

        public async Task MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _dbSet.FindAsync(notificationId);
            if (notification != null && notification.Status == NotificationStatus.Unread)
            {
                notification.Status = NotificationStatus.Read;
                notification.LastModifiedAt = DateTime.UtcNow;
                _dbSet.Update(notification);
            }
        }

        public async Task MarkAllNotificationsAsReadAsync(Guid userId)
        {
            var notifications = await _dbSet
                .Where(n => n.UserId == userId && 
                          n.Status == NotificationStatus.Unread && 
                          !n.IsDeleted)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.Status = NotificationStatus.Read;
                notification.LastModifiedAt = DateTime.UtcNow;
            }

            if (notifications.Any())
            {
                _dbSet.UpdateRange(notifications);
            }
        }
    }
}

