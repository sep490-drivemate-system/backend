using MessagingService.Application.Commons.DTOs.Notification;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Application.Interfaces
{
    public interface INotificationUseCase
    {
        Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto);
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(Guid userId, int pageNumber = 1, int pageSize = 10);
        Task<int> GetUnreadNotificationCountAsync(Guid userId);
        Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId);
        Task<bool> MarkAllNotificationsAsReadAsync(Guid userId);
    }
}

