using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.DrivingSession;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class BookingUseCase : IBookingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISystemConfigurationHttpService _systemConfigService;
        private readonly IMapper _mapper;
        private readonly IPayment _payment;
        private readonly IUser _user;

        public BookingUseCase(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, ISystemConfigurationHttpService systemConfigService, IMapper mapper, IPayment payment, IUser user)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _systemConfigService = systemConfigService;
            _mapper = mapper;
            _payment = payment;
            _user = user;
        }
        public async Task<Result<Booking>> BuyPackage(PackageBuyingDTO packageBuyingDTO, Guid driverId)
        {
            Guid id = Guid.NewGuid();
            var walletCheckResponse = await _payment.CheckWalletBooking(
               driverId,
               packageBuyingDTO.PriceAtBuyingTime,
              id, null);

            var booking = _mapper.Map<Booking>(packageBuyingDTO);
            booking.Id = id;
            booking.DriverId = driverId;
            booking.Status = BookingStatus.Purchased;
            var createdBooking = await _unitOfWork.BookingRepository.CreateAsync(booking);
            await _unitOfWork.CommitChangesAsync();

            return Result<Booking>.Success(createdBooking);

        }


        public async Task<Result<PaginatedList<BookingsDTO>>> GetBookings(BookingFilterDTO bookingFilterDTO, Guid driverId)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBookingsByDriverIdAsync(
                driverId: driverId,
                status: bookingFilterDTO.Status == 0 ? null : bookingFilterDTO.Status
            );

            if (!bookings.Any())
            {
                return Result<PaginatedList<BookingsDTO>>.Success(new PaginatedList<BookingsDTO>());
            }

            var bookingDTOs = _mapper.Map<List<BookingsDTO>>(bookings);

            var paginatedBookingDTOs = PaginatedList<BookingsDTO>.Create(
                bookingDTOs,
               bookingFilterDTO.PageNumber,
                bookingFilterDTO.PageSize
            );

            return Result<PaginatedList<BookingsDTO>>.Success(paginatedBookingDTOs);
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

            var sessionDTOs = _mapper.Map<List<DrivingSessionScheduleDTO>>(upcomingSessions);

            return Result<List<DrivingSessionScheduleDTO>>.Success(sessionDTOs);
        }

        public async Task<Result<List<DrivingSessionDetailDTO>>> GetDrivingSessions(SessionStatus status, Guid instructorId)
        {
                var sessions = await _unitOfWork.DrivingSessionRepository.GetSessionsByInstructorIdAsync(instructorId, status);
                var sessionsList = sessions.ToList();

                if (!sessionsList.Any())
                {
                    return Result<List<DrivingSessionDetailDTO>>.Success(null);
                }

                var sessionDTOs = sessions.Select(session =>
                {
                    var duration = (session.EndTime - session.StartTime).TotalHours;
                    return new DrivingSessionDetailDTO
                    {
                        Id = session.Id,
                        PackageName = session.Booking.Package?.Name,
                        Date = session.StartTime.ToString("yyyy-MM-dd"),
                        StartTime = session.StartTime.AddHours(7).ToString("HH:mm"),
                        EndTime = session.EndTime.AddHours(7).ToString("HH:mm"),
                        Duration = Math.Round(duration, 2),
                        DisplayStartLocationName = session.DisplayStartLocationName,
                        StartingLatitude = session.StartingLatitude,
                        StartingLongtitude = session.StartingLongtitude,
                        CarName = session.Booking.Car?.Name,
                        Status = session.Status,
                        DisplayEndLocationName = session.DisplayEndLocationName,
                        EndingLongtitude = session.EndingLongtitude,
                        EndingLatitude = session.EndingLatitude,
                        CreatedAt = session.CreatedAt,                        
                    };
                }).ToList();

         return   Result<List<DrivingSessionDetailDTO>>.Success(sessionDTOs);
        }


        public async Task<Result<bool>> CancelBooking(Guid booking_id, Guid user_id)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(booking_id, "DrivingSessions");

            if (booking == null || booking.IsDeleted) 
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{booking_id}"), Messages.Commons.NOTFOUND);
            }

            // Checking for time constraints (
            var timeConstraints = await _systemConfigService.GetSystemConfiguration("CancelationRefundableConstraint");
            
            if (timeConstraints == null)
            {
                Console.WriteLine("contrainst not found");
            }

            double time_constraint_value = double.Parse(timeConstraints.Value);

            if ((DateTime.Now - booking.CreatedAt).TotalDays < time_constraint_value)
            {
                decimal refundAmount;

                switch (booking.Status)
                {
                    case BookingStatus.Purchased:
                        refundAmount = booking.PriceAtBuyingTime;
                        break;
                    case BookingStatus.InUse:
                        var totalDurationUsed = (booking.DrivingSessions.Where(x => x.Status == SessionStatus.Completed).Sum(x => (x.EndTime - x.StartTime).TotalHours));
                        refundAmount = (booking.PriceAtBuyingTime / (decimal)booking.DurationWhenBought) * (decimal) (booking.DurationWhenBought - totalDurationUsed);

                        Console.WriteLine(refundAmount);
                        break;
                    default:
                        return Result<bool>.Failure(ServiceError.InvalidStateError($"{booking.Status.ToString()}"), Messages.Commons.UNHANDLED);
                }

                // Call payment service to update novice driver wallet.
                var paymentServiceHttpClient = _httpClientFactory.CreateClient("PaymentServiceClient");

                var paymentServiceResponseMessage = await paymentServiceHttpClient.PostAsJsonAsync("api/Transaction", new {
                    BookingId=booking.Id,
                    Amount=refundAmount,
                    ReferenceCode=$"{booking.Id}-Refund",

                });

                if (!paymentServiceResponseMessage.IsSuccessStatusCode)
                {
                    return Result<bool>.Failure(ServiceError.ServiceUnavailableError($"Payment Service: {paymentServiceResponseMessage.StatusCode}"), Messages.Commons.UNHANDLED);
                }

                paymentServiceResponseMessage = await paymentServiceHttpClient.PostAsJsonAsync("api/wallet/balance", new WalletBalanceDTO {
                    UserId = booking.DriverId,
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
                SessionByStatusCount = filtered_session.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => x.Count()),
                SessionStatusPercentage = filtered_session.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => (double)x.Count() / sessions.Count()),
                SessionCancelationCount = filtered_session
                    .Where(x => x.Status == SessionStatus.Cancelled && x.RescheduleRequests != null && x.RescheduleRequests.Any())
                    .GroupBy(x => 
                    {
                        var lastDeletedRequest = x.RescheduleRequests.OrderBy(r => r.CreatedAt).LastOrDefault(r => r.IsDeleted);
                        return lastDeletedRequest?.Side.ToString() ?? "Unknown";
                    })
                    .ToDictionary(x => x.Key, x => x.Count()), // For the time being, 
                SessionCancelationPercentage = filtered_session
                    .Where(x => x.Status == SessionStatus.Cancelled && x.RescheduleRequests != null && x.RescheduleRequests.Any())
                    .GroupBy(x => 
                    {
                        var lastDeletedRequest = x.RescheduleRequests.OrderBy(r => r.CreatedAt).LastOrDefault(r => r.IsDeleted);
                        return lastDeletedRequest?.Side.ToString() ?? "Unknown";
                    })
                    .ToDictionary(x => x.Key, x => (double)x.Count() / sessions.Count(x => x.Status == SessionStatus.Cancelled)),
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
            }).OrderByDescending(x => x.PackageBookCount);

            switch (filter.Type)
            {
                case StatisticTimeType.Yearly:
                    statistics.SessionTimeByDay = filtered_session.GroupBy(x => x.CreatedAt.Month.ToString()).ToDictionary(x => x.Key, x => x.Sum(u => (u.ActualEnd - u.ActualStart).TotalHours));
                    statistics.BookingByDay = filtered_booking.GroupBy(x => x.CreatedAt.Month.ToString()).ToDictionary(x => x.Key, x => x.Count());
                    break;
                case StatisticTimeType.Monthly:
                    statistics.SessionTimeByDay = filtered_session.GroupBy(x => x.CreatedAt.Day.ToString()).ToDictionary(x => x.Key.ToString(), x => x.Sum(u => (u.ActualEnd - u.ActualStart).TotalHours));
                    statistics.BookingByDay = filtered_booking.GroupBy(x => x.CreatedAt.Day.ToString()).ToDictionary(x => x.Key, x => x.Count());
                    break;
                case StatisticTimeType.Weekly:
                    statistics.SessionTimeByDay = filtered_session.GroupBy(x => x.CreatedAt.DayOfWeek.ToString()).ToDictionary(x => x.Key.ToString(), x => x.Sum(u => (u.ActualEnd - u.ActualStart).TotalHours));
                    statistics.BookingByDay = filtered_booking.GroupBy(x => x.CreatedAt.DayOfWeek.ToString()).ToDictionary(x => x.Key, x => x.Count());
                    break;
                default:
                    return Result<BookingStatisticDTO>.Failure(ServiceError.BadRequestError($"{filter.Type}"), Messages.Commons.UNHANDLED);
            }

            return Result<BookingStatisticDTO>.Success(statistics, Messages.Commons.SUCCESS);
        }

        public async Task<Result<InstructorStatisticDTO>> GetInstructorStatistic(Guid user_id, InstructorStatisticFilterDTO filter)
        {
            var userServiceHttpClient = _httpClientFactory.CreateClient("UserServiceClient");
            var userServiceResponseMessage = await userServiceHttpClient.PostAsJsonAsync<IEnumerable<Guid>>("api/users/ids", new List<Guid>() { user_id });

            if (!userServiceResponseMessage.IsSuccessStatusCode)
            {
                return Result<InstructorStatisticDTO>.Failure(ServiceError.ServiceUnavailableError($"UserService: {userServiceResponseMessage.ReasonPhrase}"), Messages.Commons.UNHANDLED);
            }

            var users = await userServiceResponseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            if (users.Value.Count() == 0 || users.Value.First().Role != UserRole.Instructor)
            {
                return Result<InstructorStatisticDTO>.Failure(ServiceError.BadRequestError($"{user_id}"), $"so user: {users.Value.Count()} . User id su dung: {user_id}");
            }
            var instructorDetail = users.Value.First();

            // Query filters
            Expression<Func<Package, bool>> packageFilter = x => !x.IsDeleted && x.InstructorId == instructorDetail.Instructor.InstructorId;
            Expression<Func<Car, bool>> carFilter = x => !x.IsDeleted && x.InstructorId == instructorDetail.Instructor.InstructorId;

            // Includes
            string packageIncludes = "Bookings,Bookings.Feedback,Bookings.DrivingSessions";

            // Expensive query bombs
            IEnumerable<Package> instructorPackage = await _unitOfWork.PackageRepository.GetAllAsync(packageFilter, include_properties: packageIncludes);
            IEnumerable<Car> instructorCar = await _unitOfWork.CarRepository.GetAllAsync(carFilter);

            IEnumerable<Booking> instructorBookings = instructorPackage.SelectMany(x => x.Bookings).Where(x => !x.IsDeleted);
            IEnumerable<DrivingSession> instructorSessions = instructorBookings.SelectMany(x => x.DrivingSessions).Where(x => !x.IsDeleted);

            // Easy to collect information
            InstructorStatisticDTO statistic = new InstructorStatisticDTO
            {
                TotalCarCount = instructorCar.Count(),
                TotalPackageCount = instructorPackage.Count(),
                TotalUpcomingSesionCount = instructorSessions.Count(x => x.Status == SessionStatus.Upcoming),
                TotalSessionByStatusCount = instructorSessions.GroupBy(x => x.Status.ToString()).ToDictionary(x => x.Key, x => x.Count()),
            };

            switch (filter.Type)
            {
                case StatisticTimeType.Yearly:
                    statistic.TotalSessionByday = instructorSessions.Where(x => x.StartTime.Year == filter.Year).GroupBy(x => x.StartTime.Month.ToString()).ToDictionary(x => x.Key, x => x.GroupBy(u => u.Status.ToString()).ToDictionary(u => u.Key, u => u.Count()));
                    break;
                case StatisticTimeType.Monthly:
                    statistic.TotalSessionByday = instructorSessions.Where(x => x.StartTime.Year == filter.Year && x.StartTime.Month == filter.Month).GroupBy(x => x.StartTime.Day.ToString()).ToDictionary(x => x.Key, x => x.GroupBy(u => u.Status.ToString()).ToDictionary(u => u.Key, u => u.Count()));
                    break;
                case StatisticTimeType.Weekly:
                    statistic.TotalSessionByday = instructorSessions.Where(x => x.StartTime.Year == filter.Year && x.StartTime.Month == filter.Month && ((x.StartTime.Day - 1) / 7) + 1 == filter.Week).GroupBy(x => x.StartTime.Day.ToString()).ToDictionary(x => x.Key, x => x.GroupBy(u => u.Status.ToString()).ToDictionary(u => u.Key, u => u.Count()));
                    break;
                default:
                    return Result<InstructorStatisticDTO>.Failure(ServiceError.BadRequestError($"{filter.Type}"), Messages.Commons.UNHANDLED);
            }

            // Recent package purchase
            var recentPurchases = instructorBookings.OrderByDescending(x => x.CreatedAt).Take(5);
            userServiceResponseMessage = await userServiceHttpClient.PostAsJsonAsync<IEnumerable<Guid>>("api/users/ids", recentPurchases.Select(x => x.DriverId));

            if (!userServiceResponseMessage.IsSuccessStatusCode)
            {
                return Result<InstructorStatisticDTO>.Failure(ServiceError.ServiceUnavailableError($"UserService: {userServiceResponseMessage.ReasonPhrase}"), Messages.Commons.UNHANDLED);
            }

            users = await userServiceResponseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            statistic.RecentPurchases = recentPurchases.Select(x => new RecentPackagePurchasesDTO
            {
                BoughtTime = x.CreatedAt,
                PackageId = x.PackageId,
                PackageName = instructorPackage.FirstOrDefault(u => u.Id == x.PackageId)?.Name ?? "",
                NoviceDriverUserId = users.Value.FirstOrDefault(u => u.UserId == x.DriverId)?.UserId ?? Guid.Empty,
                AvatarUrl = users.Value.FirstOrDefault(u => u.UserId == x.DriverId)?.AvatarUrl ?? "",
                Fullname = users.Value.FirstOrDefault(u => u.UserId == x.DriverId)?.FullName ?? "",
                PhoneNumber = users.Value.FirstOrDefault(u => u.UserId == x.DriverId)?.Phone ?? "",
            });

            // Top packages
            statistic.TopPersonalPackages = instructorBookings.GroupBy(x => x.PackageId).Select(x => new TopPersonalPackage
            {
                Id = x.Key,
                BookCount = x.Count(),
                Name = instructorPackage.FirstOrDefault(u => u.Id == x.Key)?.Name ?? "",
                Percentage = x.Count() / instructorBookings.Count()
            });

            // Top cars
            statistic.TopPersonalCars = instructorBookings.Where(x => x.CarId != null).GroupBy(x => x.CarId).Select(x => new TopPersonalCar
            {
                Id = x.Key ?? Guid.Empty,
                BookCount = x.Count(),
                Name = instructorCar.FirstOrDefault(u => u.Id == x.Key)?.Name ?? "",
                Percentage = x.Count() / instructorBookings.Where(x => x.CarId != null).Count()
            });

            return Result<InstructorStatisticDTO>.Success(statistic);
        }
    }
}
