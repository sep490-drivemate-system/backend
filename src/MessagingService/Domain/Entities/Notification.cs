using MessagingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace MessagingService.Domain.Entities
{
    public class Notification : BaseEntites
    {
        // Properties
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public string? ActionUrl { get; set; } 

        // Keys for relationships
        public Guid UserId { get; set; } 

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

