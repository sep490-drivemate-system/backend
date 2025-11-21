using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Repositories.Models
{
    public class Choice : BaseEntites
    {
        public Guid QuestionId { get; set; }
        public string ChoiceText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Relationships
        public Question Question { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}

