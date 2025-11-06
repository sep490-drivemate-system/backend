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

        public Task<FeedbackResponse> GetInstructorFeedback(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<FeedbackResponse> GetStatitic(FeedbackRequest feedbackRequest)
        {
            return await _unitOfWork.FeedbackRepository.GetStatisticListInstructor(feedbackRequest);
        }
    }
}
