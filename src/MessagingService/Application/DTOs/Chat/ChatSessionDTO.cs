namespace MessagingService.Application.DTOs.Chat
{
    public class ChatSessionsDTO
    {
        public Guid Id { get; set; }
        public Guid ToUserId { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string ToUserAvatar { get; set; }
        public string ToUserFullName { get; set; }

    }
}
