using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IDrivingSessionUseCase
    {
        Task<Result<ICollection<DrivingSession>>> GetAllDrivingSession(SessionStatus sessionStatus);
        Task<Result<bool> CreateDrivingSession(DrivingSessionCreationDTO drivingSessionCreationDTO);

        Task<Result<IEnumerable<DrivingSessionDTO>>> GetUserSessions(Guid user_id, SessionFilterDTO session_filter);

        Task<Result<IEnumerable<DrivingSessionDTO>>> GetUserSessionsWithChangeRequest(Guid user_id);

        Task<Result<DrivingSessionDTO>> GetSessionDetail(Guid session_id);

        Task<Result<bool>> CancelSession(Guid session_id, SessionCancelRequestDTO cancelationDTO);

        Task<Result<bool>> RescheduleSession(Guid session_id, SessionRescheduleRequestDTO rescheduleDTO);
    }
}

