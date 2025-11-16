namespace MessagingService.Application.DTOs.Chat
{
    public class ConversationResponseDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool IsGroupChat { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
        public List<ParticipantDTO> Participants { get; set; } = new();
    }

    public class ParticipantDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
    }
}

