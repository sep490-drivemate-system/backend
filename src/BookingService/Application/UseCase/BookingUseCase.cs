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

        public async Task<Result<bool>> CreateBooking(BookingDTO bookingDTO, Guid userId)
        {
            //// 1. Check payment
            //var walletCheckResponse = await _payment.CheckWalletBooking(
            //    userId, 
            //    bookingDTO.Price);

            //if (!walletCheckResponse.IsPayment)
            //{
            //    return Result<bool>.Failure(ServiceError.BadRequestError(Messages.Booking.INSUFFICENTCREDIT));
            //}

            //// 2. Add All booking information into booking and time range
            //var booking = _mapper.Map<Booking>(bookingDTO);


            //// Create booking first
            //var createdBooking = await _unitOfWork.BookingRepository.CreateAsync(booking);

            //// 3. Add time ranges for the booking
            //var timeRanges = new List<TimeRange>();
            //foreach (var timeRangeDto in bookingDTO.TimeRanges)
            //{
            //    var timeRange = _mapper.Map<TimeRange>(timeRangeDto);
            //    timeRange.BookingId = createdBooking.Id;
            //    timeRanges.Add(timeRange);
            //}

            //foreach (var timeRange in timeRanges)
            //{
            //    await _unitOfWork.TimeRangeRepository.CreateAsync(timeRange);
            //}



            //foreach (var drivingSkillDto in bookingDTO.DrivingSkills)
            //{
            //    // Check if driving skill exists
            //    var existingSkill = await _unitOfWork.SkillRepository.GetByIdAsync(drivingSkillDto.DrivingSkillId);
            //    if (existingSkill != null)
            //    {
            //        // Add to booking's driving skills collection
            //        createdBooking.DrivingSkills.Add(existingSkill);
            //    }
            //}

            //// 5. Add road types (many-to-many relationship)


            //foreach (var roadTypeDto in bookingDTO.RoadTypes)
            //{
            //    // Check if road type exists
            //    var existingRoadType = await _unitOfWork.RoadTypeRepository.GetByIdAsync(roadTypeDto.RoadTypeId);
            //    if (existingRoadType != null)
            //    {
            //        // Add to booking's road types collection
            //        createdBooking.RoadTypes.Add(existingRoadType);
            //    }
            //}

            //await _unitOfWork.BookingRepository.Update(createdBooking);

            //// Save all changes to database
            //await _unitOfWork.CommitChanges();

            //return Result<bool>.Success(true);

            throw new NotImplementedException();
        }
    }
}
