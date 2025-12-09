using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class Answer : BaseEntites
    {
        public Guid AttemptId { get; set; }
        public Guid ChoiceId { get; set; }
        public bool IsCorrect { get; set; }

        // Relationships
        public Attempt Attempt { get; set; } = null!;
        public Choice Choice { get; set; } = null!;
    }
}

