using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.DrivingSession;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Commons.Mapping;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using BookingService.Infrastructure.Messaging.Interface;
using BookingService.Infrastructure.Persistence.Context;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class BookingUseCase : IBookingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;
        private readonly IUser _user;

        public BookingUseCase(IUnitOfWork unitOfWork, IMapper mapper, IPayment payment, IUser user)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payment = payment;
            _user = user;
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

        public async Task<Result<List<BookingsDTO>>> GetBookings(BookingStatus status, Guid driverId)
        {
            Expression<Func<Booking, bool>> filter = status == 0
                ? b => b.DriverId == driverId && !b.IsDeleted
                : b => b.DriverId == driverId && b.Status == status && !b.IsDeleted;

            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(
                filter: filter,
                orderBy: null,
                include_properties: "Package,DrivingSessions,Car"
            );

            if (!bookings.Any())
            {
                return Result<List<BookingsDTO>>.Success(new List<BookingsDTO>());
            }

            var bookingDTOs = _mapper.Map<List<BookingsDTO>>(bookings);

            return Result<List<BookingsDTO>>.Success(bookingDTOs);
        }

        public async Task<Result<List<DrivingSessionScheduleDTO>>> GetUpcomingDrivingSessions(Guid instructorId)
        {
            // Get all bookings for this instructor
            var instructorBookings = await _unitOfWork.BookingRepository.GetAllAsync(
                filter: b => b.InstructorId == instructorId && !b.IsDeleted,
                include_properties: "DrivingSessions"
            );

            if (!instructorBookings.Any())
            {
                return Result<List<DrivingSessionScheduleDTO>>.Success(new List<DrivingSessionScheduleDTO>());
            }

            // Get all upcoming sessions from these bookings
            var bookingIds = instructorBookings.Select(b => b.Id).ToList();
            
            var upcomingSessions = await _unitOfWork.DrivingSessionRepository.GetAllAsync(
                filter: ds => bookingIds.Contains(ds.BookingId) && 
                             ds.Status == SessionStatus.Upcoming && 
                             !ds.IsDeleted,
                orderBy: q => q.OrderBy(ds => ds.StartTime)
            );

            var sessionDTOs = upcomingSessions.Select(s => new DrivingSessionScheduleDTO
            {
                StartTime = s.StartTime,
                EndTime = s.EndTime,
            }).ToList();

            return Result<List<DrivingSessionScheduleDTO>>.Success(sessionDTOs);
        }

        public async Task<Result<List<DrivingSessionDetailDTO>>> GetDrivingSessions(SessionStatus status, Guid instructorId)
        {
                var sessions = await _unitOfWork.DrivingSessionRepository.GetSessionsByInstructorIdAsync(instructorId, status);
                var sessionsList = sessions.ToList();

                if (!sessionsList.Any())
                {
                    return Result<List<DrivingSessionDetailDTO>>.Success(new List<DrivingSessionDetailDTO>());
                }

                // Get unique novice driver IDs from all sessions
                var noviceDriverIds = sessions.Select(s => s.Booking.DriverId).Distinct().ToList();

                // Get novice driver info from UserService
                var noviceDriverInfos = await _user.GetBatchNoviceDriverInfo(noviceDriverIds);

                var sessionDTOs = sessions.Select(session =>
                {
                    var duration = (session.EndTime - session.StartTime).TotalHours;
                    var hasRoute = session.SessionRoutes?.Any(sr => !sr.IsDeleted) ?? false;
                    var noviceDriverInfo = noviceDriverInfos.GetValueOrDefault(session.Booking.DriverId);

                    return new DrivingSessionDetailDTO
                    {
                        Id = session.Id,
                        PackageId = session.Booking.PackageId,
                        PackageName = session.Booking.Package?.Name ?? "Unknown Package",
                        NoviceDriverName = noviceDriverInfo?.Fullname ?? "Unknown Driver",
                        NoviceAvatar = noviceDriverInfo?.AvatarUrl,
                        Date = session.StartTime.ToString("yyyy-MM-dd"),
                        StartTime = session.StartTime.ToString("HH:mm"),
                        EndTime = session.EndTime.ToString("HH:mm"),
                        Duration = Math.Round(duration, 2),
                        Location = session.DisplayName,
                        StartingLatitude = session.StartingLatitude,
                        StartingLongtitude = session.StartingLongtitude,
                        VehicleId = session.Booking.CarId,
                        VehicleName = session.Booking.Car?.Name,
                        Status = session.Status,
                        StatusDisplayString = GetStatusDisplayString(session.Status),
                        CreatedAt = session.CreatedAt,
                        HasRoute = hasRoute,
                        PriceForCar = session.PriceForCar
                    };
                }).ToList();

         return   Result<List<DrivingSessionDetailDTO>>.Success(sessionDTOs);
        }

        private string GetStatusDisplayString(SessionStatus status)
        {
            return status switch
            {
                SessionStatus.Planning => "planning",
                SessionStatus.Upcoming => "upcoming",
                SessionStatus.InProgress => "in-progress",
                SessionStatus.Completed => "completed",
                SessionStatus.Cancelled => "cancelled",
                SessionStatus.Reschedule => "reschedule",
                _ => status.ToString().ToLower()
            };
        }
    }
}
