using BookingService.Application.Commons.DTOs.Feedbacks;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;

namespace BookingService.Application.Interfaces
{
    public interface IFeedbackUseCase
    {
        Task<InstructorOverviewFeedbackResponse> GetStatitic(Guid instructorId );
        Task<InstructorFeedbackDTO> GetInstructorFeedback(Guid id);
    }
}

