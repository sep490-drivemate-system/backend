namespace MessagingService.Application.DTOs.Chat
{
    public class SendMessageDTO
    {
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
    }
}

