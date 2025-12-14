using ResourceService.Application.Commons.DTOs.QA;
using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Interfaces.Services
{
    public interface IQAService
    {
        Task<Result<bool>> CreateQuestion(CreateQuestionDTO request, Guid authorId);
        Task<Result<QAsDTO>> GetQuestions(FilterQADTO filter);
        Task<Result<bool>> UpdateQuestionStatus(Guid questionId, QaStatus status);
        Task<Result<bool>> AnswerQuestion(AnswerQuestionDTO answerQuestionDTO, Guid authorId);
        Task<Result<bool>> DeleteQuestion(Guid questionId);
    }
}
