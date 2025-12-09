using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class Question : BaseEntites
    {
        public Guid QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Relationships
        public Quiz Quiz { get; set; } = null!;
        public ICollection<Choice> Choices { get; set; } = new List<Choice>();
    }
}

