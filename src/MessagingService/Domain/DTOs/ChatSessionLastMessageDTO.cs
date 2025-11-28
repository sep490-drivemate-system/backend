using System;

namespace MessagingService.Domain.DTOs
{
    public class ChatSessionLastMessageDTO
    {
        public Guid Id { get; set; }
        public Guid PartnerId { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public DateTime? LastMessageAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}

