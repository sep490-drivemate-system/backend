using System;
using System.Collections.Generic;

namespace ResourceService.Services.DTOs.Quizzes
{
    public class QuizAttemptRequestDTO
    {
        public IEnumerable<QuizAttemptAnswerDTO> Answers { get; set; } = [];
    }

    public class QuizAttemptAnswerDTO
    {
        public Guid QuestionId { get; set; }
        public Guid ChoiceId { get; set; }
    }

    public class QuizAttemptResultDTO
    {
        public Guid AttemptId { get; set; }
        public Guid QuizId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Score { get; set; }
        public IEnumerable<QuizAttemptAnswerResultDTO> Answers { get; set; } = [];
    }

    public class QuizAttemptAnswerResultDTO
    {
        public Guid QuestionId { get; set; }
        public Guid ChoiceId { get; set; }
        public bool IsCorrect { get; set; }
    }
}

