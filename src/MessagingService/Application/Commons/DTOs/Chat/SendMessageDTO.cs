namespace MessagingService.Application.Commons.DTOs.Chat
{
    public class SendMessageDTO
    {
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public Guid RecipientId { get; set; }
    }
}

