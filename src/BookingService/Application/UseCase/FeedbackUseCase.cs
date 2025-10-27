using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;

namespace BookingService.Application.UseCase
{
    public class FeedbackUseCase : IFeedbackUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public FeedbackUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<FeedbackResponse> GetStatitic(FeedbackRequest feedbackRequest)
        {
            return await _unitOfWork.FeedbackRepository.GetStatiicListInstructor(feedbackRequest);
        }
    }
}
