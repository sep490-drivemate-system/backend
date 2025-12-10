using MessagingService.Domain.Enum;

namespace MessagingService.Application.Commons.DTOs.Notification
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public string? ActionUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

