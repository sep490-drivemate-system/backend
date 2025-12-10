using MessagingService.Domain.Enum;

namespace MessagingService.Application.Commons.DTOs.Notification
{
    public class CreateNotificationDTO
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public string? ActionUrl { get; set; }
        public Guid? RelatedEntityId { get; set; }
    }
}

