using ResourceService.Services.DTOs.Quizzes;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Interfaces
{
    public interface IQuizService
    {
        Task<Result<QuizDetailDTO>> GetQuizDetailWithId(Guid id);
        Task<Result<IEnumerable<QuizViewDTO>>> GetAllQuiz(string? tag = null, int duration = 0);
        Task<Result<bool>> CreateQuiz(QuizCreateOrUpdateDTO quiz, Guid inspectorId);
        Task<Result<bool>> UpdateQuiz(Guid id, QuizCreateOrUpdateDTO quiz);
        Task<Result<bool>> DeleteQuiz(Guid id);
        Task<Result<QuizAttemptStartResultDTO>> StartQuizAttempt(Guid quizId, Guid userId);
        Task<Result<QuizAttemptResultDTO>> SubmitQuizAttempt(Guid attemptId, Guid userId, QuizAttemptRequestDTO attemptRequest);
    }
}
