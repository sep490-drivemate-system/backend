using ResourceService.Services.DTOs.Quizzes;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Services.Interfaces
{
    public interface IQuizService
    {
        Task<Result<QuizDetailDTO>> GetQuizDetailWithId(Guid id);
        Task<Result<IEnumerable<QuizViewDTO>>> GetAllQuiz(string? tag = null, int duration = 0);
        Task<Result<bool>> CreateQuiz(QuizCreateOrUpdateDTO quiz);
        Task<Result<bool>> UpdateQuiz(Guid id, QuizCreateOrUpdateDTO quiz);
        Task<Result<bool>> DeleteQuiz(Guid id);
    }
}
