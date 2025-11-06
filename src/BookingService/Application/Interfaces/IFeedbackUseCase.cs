using SharedLibrary.SharedKernel.Http.DTOs.Feedback;

namespace BookingService.Application.Interfaces
{
    public interface IFeedbackUseCase
    {
        Task<FeedbackResponse> GetStatitic(FeedbackRequest feedbackRequest);
        Task<FeedbackResponse> GetInstructorFeedback(Guid id);
    }
}
