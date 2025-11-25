namespace MessagingService.Application.Commons.DTOs.Chat
{
    public class MessageResponseDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAvatar { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

