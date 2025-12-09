using ResourceService.Application.Commons.DTOs.Quizzes;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Interfaces.Services
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
        Task<Result<IEnumerable<QuizAttemptHistoryDTO>>> GetQuizAttemptHistory(Guid userId);
        Task<Result<QuizAttemptDetailDTO>> GetQuizAttemptDetail(Guid attemptId, Guid userId);
    }
}
