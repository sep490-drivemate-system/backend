namespace ResourceService.Services.DTOs.Quizzes
{
    public class QuizCreateOrUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } // Calculated in minutes
        public string Tag { get; set; }
        public IEnumerable<QuestionCreateOrUpdateDTO> Questions { get; set; }
    }

    public class QuestionCreateOrUpdateDTO
    {
        public Guid? Id { get; set; }
        public string QuestionText { get; set; }
        public IEnumerable<ChoiceCreateOrUpdateDTO> Choice { get; set; }
    }

    public class ChoiceCreateOrUpdateDTO
    {
        public Guid? Id { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
