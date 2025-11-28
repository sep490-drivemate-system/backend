using System;

namespace MessagingService.Application.Commons.DTOs.Chat
{
    public class CreateChatSessionDTO
    {
        public Guid InstructorId { get; set; }
        public Guid NoviceDriverId { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}

