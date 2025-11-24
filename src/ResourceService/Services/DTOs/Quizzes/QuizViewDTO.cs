namespace ResourceService.Services.DTOs.Quizzes
{
    public class QuizViewDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Tag { get; set; }
        public int QuestionCount { get; set; }
    }

    public class QuizDetailDTO: QuizViewDTO
    {
        public IEnumerable<QuestionDetailDTO> Questions { get; set; }
    }

    public class QuestionDetailDTO
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public IEnumerable<ChoiceDetailDTO> Choices { get; set; }
    }

    public class ChoiceDetailDTO
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
