using BookingService.Application.Commons.DTOs.Feedbacks;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IFeedbackUseCase
    {
        Task<InstructorOverviewFeedbackResponse> GetStatitic(Guid instructorId );
        Task<Result<bool>> SaveFeedback(FeedbackCreationDTO feedbackCreationDTO,Guid driverId);
        Task<Dictionary<Guid, InstructorOverviewFeedbackResponse>> GetBatchStatistics(List<Guid> instructorIds);
    }
}

