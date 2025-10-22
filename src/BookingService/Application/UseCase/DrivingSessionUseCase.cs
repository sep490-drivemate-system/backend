using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.UseCase
{
    public class DrivingSessionUseCase : IDrivingSessionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DrivingSessionUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ICollection<DrivingSession>>> GetAllDrrivingSession(SessionStatus sessionStatus)
        {

                var drivingSessions = await _unitOfWork.DrivingSessionRepository.GetAllByStatus(sessionStatus);
                
                var result = drivingSessions.ToList();
                
                return Result<ICollection<DrivingSession>>.Success(result);
           
        }
    }
}
