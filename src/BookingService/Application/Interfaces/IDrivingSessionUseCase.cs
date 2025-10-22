using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IDrivingSessionUseCase
    {
        Task<Result<ICollection<DrivingSession>>> GetAllDrrivingSession(SessionStatus sessionStatus);
    }
}

