using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Application.Interfaces
{
    public interface IBookingUseCase
    {
        Task<Result<Booking>> CreateBooking(BookingDTO bookingDTO, Guid driverId);
    }
}
