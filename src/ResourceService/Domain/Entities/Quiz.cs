using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class Quiz : BaseEntites
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int QuizDuration { get; set; } // Duration in minutes
        public string Tag { get; set; } = string.Empty; // Topic/tag like "đèn giao thông", "biển báo", "tốc độ"
        public Guid InspectorId { get; set; } // Inspector who created this quiz
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Relationships
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
    }
}

