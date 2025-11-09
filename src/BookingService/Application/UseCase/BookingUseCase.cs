using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Infrastructure.Messaging.Interface;
using BookingService.Infrastructure.Persistence.Context;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;


namespace BookingService.Application.UseCase
{
    public class BookingUseCase : IBookingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;

        public BookingUseCase(IUnitOfWork unitOfWork, IMapper mapper, IPayment payment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payment = payment;
        }

        public async Task<Result<Booking>> CreateBooking(BookingDTO bookingDTO, Guid userId)
        {

                var walletCheckResponse = await _payment.CheckWalletBooking(
                    userId,
                    bookingDTO.PriceAtBuyingTime,
                    bookingDTO.Id);


                if (!walletCheckResponse.IsPayment)
                {
                    return Result<Booking>.Failure(ServiceError.BadRequestError(Messages.Booking.INSUFFICENTCREDIT));
                }

                var booking = _mapper.Map<Booking>(bookingDTO);

                var createdBooking = await _unitOfWork.BookingRepository.CreateAsync(booking);
                await _unitOfWork.CommitChangesAsync();

                return Result<Booking>.Success(createdBooking);
           
        }
    }
}
