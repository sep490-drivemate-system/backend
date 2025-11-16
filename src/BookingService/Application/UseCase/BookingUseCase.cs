using AutoMapper;
using Azure.Core;
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
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class BookingUseCase : IBookingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;
        private readonly IUser _user;

        public BookingUseCase(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IMapper mapper, IPayment payment, IUser user)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
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
            var bookings = await _unitOfWork.BookingRepository.GetBookingsByDriverIdAsync(
                driverId: driverId,
                status: status == 0 ? null : status
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

        public async Task<Result<bool>> CancelBooking(Guid booking_id, Guid user_id)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(booking_id, "DrivingSessions");

            if (booking == null || booking.IsDeleted) 
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{booking_id}"), Messages.Commons.NOTFOUND);
            }

            // Validate booking status
            if (booking.Status == BookingStatus.CancellationWithRefund || booking.Status == BookingStatus.CancellationWithRefund || booking.Status == BookingStatus.Used)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"{booking.Status.ToString()}"), Messages.Commons.UNHANDLED);
            }

            // Validate novice driver
            var userServiceHttpClient = _httpClientFactory.CreateClient("UserServiceClient");
            var userServiceResponseMessage = await userServiceHttpClient.PostAsJsonAsync<IEnumerable<Guid>>("api/users/ids", new List<Guid>() { user_id });
            
            if (!userServiceResponseMessage.IsSuccessStatusCode)
            {
                return Result<bool>.Failure(ServiceError.ServiceUnavailableError($"UserService:"), Messages.Commons.UNHANDLED);
            }

            var users = await userServiceResponseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();
            if (users.Value.Count() == 0 || users.Value.First().NoviceDriver.NoviceDriverId != booking.DriverId)
            {
                return Result<bool>.Failure(ServiceError.BadRequestError($"{booking_id}"), Messages.Booking.NOTOWNEDBOOKING);
            }

            // Actual refund calculation
            if ((DateTime.Now - booking.CreatedAt).TotalDays < 30)
            {
                decimal refundAmount;

                switch (booking.Status)
                {
                    case BookingStatus.Purchased:
                        refundAmount = booking.PriceAtBuyingTime;
                        break;
                    case BookingStatus.InUse:
                        var totalDurationUsed = (booking.DrivingSessions.Where(x => x.Status == SessionStatus.Completed).Sum(x => (x.StartTime - x.EndTime).TotalHours));
                        refundAmount = (booking.PriceAtBuyingTime / (decimal)booking.DurationWhenBought) * (decimal) (booking.DurationWhenBought - totalDurationUsed);
                        break;
                    default:
                        return Result<bool>.Failure(ServiceError.InvalidStateError($"{booking.Status.ToString()}"), Messages.Commons.UNHANDLED);
                }

                // Call payment service to update novice driver wallet.
                var paymentServiceHttpClient = _httpClientFactory.CreateClient("PaymentServiceClient");

                var paymentServiceResponseMessage = await paymentServiceHttpClient.PostAsJsonAsync("api/wallet/balance", new WalletBalanceDTO {
                    UserId = user_id,
                    Balance = refundAmount,
                });

                if (!paymentServiceResponseMessage.IsSuccessStatusCode)
                {
                    return Result<bool>.Failure(ServiceError.ServiceUnavailableError($"Payment Service: {paymentServiceResponseMessage.StatusCode}"), Messages.Commons.UNHANDLED);
                }

                booking.Status = BookingStatus.CancellationWithRefund;
            }
            else
            {
                booking.Status = BookingStatus.CancellationWithoutRefund;
            }

            // Cancel all sessions in the bought package.
            foreach (var session in booking.DrivingSessions)
            {
                if (session.Status != SessionStatus.Completed || session.Status != SessionStatus.Cancelled)
                    session.Status = SessionStatus.Cancelled;
            }

            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }

        public async Task<Result<BookingStatisticDTO>> GetBookingStatistic(BookingStatisticFilterDTO filter)
        {
            Func<Booking, bool> bookingQueryFilter;
            Func<Car, bool> carQueryFilter;
            Func<Package, bool> packageQueryFilter;
            Func<DrivingSession, bool> sessionQueryFilter;

            switch (filter.Type)
            {
                case StatisticTimeType.Yearly:
                    bookingQueryFilter = x => x.CreatedAt.Year == filter.Year;
                    carQueryFilter = x => x.CreatedAt.Year == filter.Year;
                    packageQueryFilter = x => x.CreatedAt.Year == filter.Year;
                    sessionQueryFilter = x => x.CreatedAt.Year == filter.Year;
                    break;
                case StatisticTimeType.Monthly:
                    bookingQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month;
                    carQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month;
                    packageQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month;
                    sessionQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month;
                    break;
                case StatisticTimeType.Weekly:
                    bookingQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month && ((x.CreatedAt.Day - 1) / 7) + 1 == filter.Week;
                    carQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month && ((x.CreatedAt.Day - 1) / 7) + 1 == filter.Week;
                    packageQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month && ((x.CreatedAt.Day - 1) / 7) + 1 == filter.Week;
                    sessionQueryFilter = x => x.CreatedAt.Year == filter.Year && x.CreatedAt.Month == filter.Month && ((x.CreatedAt.Day - 1) / 7) + 1 == filter.Week;
                    break;
                default:
                    return Result<BookingStatisticDTO>.Failure(ServiceError.BadRequestError($"{filter.Type}"), Messages.Commons.UNHANDLED);
            }

            string bookings_includes = "DrivingSessions,Feedback";

            var packages = await _unitOfWork.PackageRepository.GetAllAsync(filter: x => !x.IsDeleted);
            var cars = await _unitOfWork.CarRepository.GetAllAsync(filter: x=> !x.IsDeleted);
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(filter: x => !x.IsDeleted, include_properties: bookings_includes);
            var sessions = bookings.SelectMany(x => x.DrivingSessions); // This gonna help reducing the database call

            // filtered bomb which cause more bombing
            var filtered_package = packages.Where(packageQueryFilter);
            var filtered_car = cars.Where(carQueryFilter);
            var filtered_session = sessions.Where(sessionQueryFilter);
            var filtered_booking = bookings.Where(bookingQueryFilter);

            BookingStatisticDTO statistics = new BookingStatisticDTO
            {
                Type = filter.Type,
                Year = filter.Year,
                Month = filter.Month,
                Week = filter.Week,
                TotalBookingCount = bookings.Count,
                TotalCarCount = cars.Count,
                TotalPackageCount = packages.Count,
                TotalSessionCount = bookings.Sum(x => x.DrivingSessions.Count),
                TotalCancelationCount = bookings.Sum(x => x.DrivingSessions.Count(x => !x.IsDeleted && x.Status == SessionStatus.Cancelled)),
                BookingByStatusCount = filtered_booking.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => x.Count()),
                BookingStatusPercentage = filtered_booking.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => (double)x.Count() / bookings.Count),
                SessionByStatusCount = sessions.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => x.Count()),
                SessionStatusPercentage = filtered_session.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => (double)x.Count() / sessions.Count()),
                SessionCancelationCount = filtered_session.Where(x => x.Status == SessionStatus.Cancelled).GroupBy(x => x.RescheduleRequests.OrderBy(x => x.CreatedAt).Last(x => x.IsDeleted).Side.ToString()).ToDictionary(x => x.Key, x => x.Count()), // For the time being, 
                SessionCancelationPercentage = filtered_session.Where(x => x.Status == SessionStatus.Cancelled).GroupBy(x => x.RescheduleRequests.OrderBy(x => x.CreatedAt).Last(x => x.IsDeleted).Side.ToString()).ToDictionary(x => x.Key, x => (double)x.Count() / sessions.Count(x => x.Status == SessionStatus.Cancelled)),
            };

            statistics.TopCars = bookings.Where(x => x.Feedback != null && x.CarId != null).GroupBy(x => x.CarId).Select(x => new TopCar
            {
                CarName = cars.FirstOrDefault(u => u.Id == x.Key)?.Name ?? "",
                CarBookCount = x.Count(),
                AverageRating = x.Average(u => u.Feedback?.CarRating ?? 0)
            });
            statistics.TopPackages = bookings.Where(x => x.Feedback != null).GroupBy(x => x.PackageId).Select(x => new TopPackage
            {
                PackageName = packages.FirstOrDefault(u => u.Id == x.Key)?.Name ?? "",
                PackageBookCount = x.Count(),
                AverageRating = 0
            });

            switch (filter.Type)
            {
                case StatisticTimeType.Yearly:
                    statistics.BookingByStatusCount = filtered_session.GroupBy(x => x.CreatedAt.Month.ToString()).ToDictionary(x => x.Key, x => x.Count());
                    statistics.SessionTimeByDay = filtered_session.GroupBy(x => x.CreatedAt.Month.ToString()).ToDictionary(x => x.Key, x => x.Sum(u => (u.ActualEnd - u.ActualStart).TotalHours));
                    break;
                case StatisticTimeType.Monthly:
                    statistics.BookingByStatusCount = filtered_session.GroupBy(x => x.CreatedAt.Day.ToString()).ToDictionary(x => x.Key.ToString(), x => x.Count());
                    statistics.SessionTimeByDay = filtered_session.GroupBy(x => x.CreatedAt.Day.ToString()).ToDictionary(x => x.Key.ToString(), x => x.Sum(u => (u.ActualEnd - u.ActualStart).TotalHours));
                    break;
                case StatisticTimeType.Weekly:
                    statistics.BookingByStatusCount = filtered_session.GroupBy(x => x.CreatedAt.DayOfWeek.ToString()).ToDictionary(x => x.Key.ToString(), x => x.Count());
                    statistics.SessionTimeByDay = filtered_session.GroupBy(x => x.CreatedAt.DayOfWeek.ToString()).ToDictionary(x => x.Key.ToString(), x => x.Sum(u => (u.ActualEnd - u.ActualStart).TotalHours));
                    break;
                default:
                    return Result<BookingStatisticDTO>.Failure(ServiceError.BadRequestError($"{filter.Type}"), Messages.Commons.UNHANDLED);
            }

            return Result<BookingStatisticDTO>.Success(statistics, Messages.Commons.SUCCESS);
        }
    }
}
