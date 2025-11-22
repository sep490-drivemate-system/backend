using MessagingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace MessagingService.Domain.Entities
{
    public class Message : BaseEntites
    {
        // Properties
        public string Content { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.Sent;

        // Keys for relationships
        public Guid ChatSessionId { get; set; }
        public Guid SenderId { get; set; } 

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual ChatSession? Conversation { get; set; }
    }
}

