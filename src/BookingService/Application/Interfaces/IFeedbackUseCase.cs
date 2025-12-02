using BookingService.Application.Commons.DTOs.Feedbacks;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IFeedbackUseCase
    {
        Task<Result<bool>> SaveFeedback(FeedbackCreationDTO feedbackCreationDTO,Guid driverId);
        Task<Result<IEnumerable<InstructorFeedbackDTO>>> GetInstructorFeedbacks(Guid instructor_id);
        Task<Result<IEnumerable<CarFeedbackDTO>>> GetCarFeedbacks(Guid car_id);

        #region Functions for other services
        Task<InstructorOverviewFeedbackResponse> GetStatitic(Guid instructorId);
        Task<Dictionary<Guid, InstructorOverviewFeedbackResponse>> GetBatchStatistics(List<Guid> instructorIds);
        #endregion
    }
}

