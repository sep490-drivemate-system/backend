using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.DrivingSession;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IBookingUseCase
    {
        Task<Result<Booking>> CreateBooking(BookingDTO bookingDTO, Guid driverId);
        Task<Result<List<BookingsDTO>>> GetBookings(BookingStatus status, Guid driverId);
        Task<Result<List<DrivingSessionScheduleDTO>>> GetUpcomingDrivingSessions(Guid instructorId);
    }
}
