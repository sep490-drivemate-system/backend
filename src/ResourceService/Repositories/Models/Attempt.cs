using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Repositories.Models
{
    public class Attempt : BaseEntites
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Relationships
        public Quiz Quiz { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}

