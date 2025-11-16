using SharedLibrary.SharedKernel.Entities;

namespace MessagingService.Domain.Entities
{
    public class ChatSession : BaseEntites
    {
        // Properties
        public Guid NoviceDriverId  { get; set; }
        public Guid InstructorId { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual ICollection<Message>? Messages { get; set; }
    }
}

