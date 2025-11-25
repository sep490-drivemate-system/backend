using MessagingService.Application.Commons.DTOs.Notification;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Application.Interfaces
{
    public interface INotificationUseCase
    {
        Task<Result<NotificationResponseDTO>> CreateNotificationAsync(CreateNotificationDTO dto);
        Task<Result<IEnumerable<NotificationResponseDTO>>> GetUserNotificationsAsync(Guid userId, int pageNumber = 1, int pageSize = 50);
        Task<Result<int>> GetUnreadNotificationCountAsync(Guid userId);
        Task<Result<bool>> MarkNotificationAsReadAsync(Guid notificationId, Guid userId);
        Task<Result<bool>> MarkAllNotificationsAsReadAsync(Guid userId);
    }
}

