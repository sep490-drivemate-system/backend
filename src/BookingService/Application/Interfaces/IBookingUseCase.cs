using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.DrivingSession;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IBookingUseCase
    {
        Task<Result<bool>> CancelBooking(Guid booking_id, Guid user_id);
        Task<Result<List<BookingsDTO>>> GetBookings(BookingStatus status, Guid driverId);
        Task<Result<Booking>> BuyPackage(PackageBuyingDTO packageBuyingDTO, Guid driverId);
        Task<Result<List<DrivingSessionScheduleDTO>>> GetUpcomingDrivingSessions(Guid instructorId);
        Task<Result<List<DrivingSessionDetailDTO>>> GetDrivingSessions(SessionStatus status,Guid instructorId);
        Task<Result<BookingStatisticDTO>> GetBookingStatistic(BookingStatisticFilterDTO filter);
        Task<Result<InstructorStatisticDTO>> GetInstructorStatistic(Guid user_id, InstructorStatisticFilterDTO filter);
    }
}
