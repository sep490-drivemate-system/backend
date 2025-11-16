using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using Newtonsoft.Json;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;
using System.Net.Http;

namespace BookingService.Application.UseCase
{
    public class DrivingSessionUseCase(IUnitOfWork unitOfWok, IJwtService jwtService, IUser userService, IMapper mapper, IHttpClientFactory httpClientFactory, IPayment paymentService) : IDrivingSessionUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWok;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUser _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IPayment _paymentService = paymentService;

        public async Task<Result<ICollection<DrivingSession>>> GetAllDrivingSession(SessionStatus sessionStatus)
        {

            var drivingSessions = await _unitOfWork.DrivingSessionRepository.GetAllByStatus(sessionStatus);

            var result = drivingSessions.ToList();

            return Result<ICollection<DrivingSession>>.Success(result);

        }

        public async Task<Result<bool>> CreateDrivingSession(DrivingSessionCreationDTO drivingSessionCreationDTO)
        {
            // Validate booking exists
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(drivingSessionCreationDTO.BookingId);
            if (booking == null)
            {
                return Result<bool>.Failure(
                    ServiceError.NotFoundError($"Booking {drivingSessionCreationDTO.BookingId}"),
                    Messages.Commons.NOTFOUND);
            }

            // Validate booking status
            //if (booking.Status != BookingStatus.Planned && booking.Status != BookingStatus.Planned)
            //{
            //    return Result<bool>.Failure(
            //        ServiceError.InvalidStateError($"Booking status: {booking.Status}"),
            //        "Booking must be confirmed or planned to create driving session");
            //}
            DateTime startTimeUtc = drivingSessionCreationDTO.StartTime.Kind == DateTimeKind.Utc
        ? drivingSessionCreationDTO.StartTime
        : drivingSessionCreationDTO.StartTime.ToUniversalTime();


            DateTime endTimeUtc = startTimeUtc.AddHours(drivingSessionCreationDTO.Duration);
            // Calculate end time based on duration

            // Create new driving session entity with temporary ID
            var sessionId = Guid.NewGuid();
            var drivingSession = new DrivingSession
            {
                Id = sessionId,
                BookingId = drivingSessionCreationDTO.BookingId,
                StartTime = startTimeUtc,
                EndTime = endTimeUtc,
                PriceForCar = drivingSessionCreationDTO.PriceForCar,
                StartingLatitude = drivingSessionCreationDTO.StartingLatitude,
                StartingLongtitude = drivingSessionCreationDTO.StartingLongtitude,
                NoviceDriverNote = drivingSessionCreationDTO.SessionNote,
                Status = SessionStatus.Planning,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                DisplayName = drivingSessionCreationDTO.DisplayName,
                IsDeleted = false,
                // Initialize default values for fields that will be updated later
                ActualStart = DateTime.MinValue,
                ActualEnd = DateTime.MinValue,
                TotalDistance = 0,
                AverageSpeed = 0,
                EndingLatitude = 0,
                EndingLongtitude = 0
            };

            try
            {
                // Check if PriceForCar has value, then call PaymentService
                if (drivingSessionCreationDTO.PriceForCar.HasValue && drivingSessionCreationDTO.PriceForCar.Value > 0)
                {
                    try
                    {
                        var paymentResponse = await _paymentService.CheckWalletBooking(
                            booking.DriverId,
                            drivingSessionCreationDTO.PriceForCar.Value,
                            drivingSessionCreationDTO.BookingId,
                            sessionId
                        );

                        if (!paymentResponse.IsPayment)
                        {
                            // Insufficient balance
                            return Result<bool>.Failure(
                                ServiceError.BadRequestError("Insufficient wallet balance"),
                                paymentResponse.Message ?? "Số dư trong ví không đủ để thanh toán cho xe. Vui lòng nạp thêm tiền.");
                        }

                        // Payment successful, continue to create session
                    }
                    catch (Exception paymentEx)
                    {
                        return Result<bool>.Failure(
                            ServiceError.UnhandledException($"Payment service error: {paymentEx.Message}"),
                            "Không thể kết nối đến dịch vụ thanh toán. Vui lòng thử lại sau.");
                    }
                }

                // Create driving session in database
                await _unitOfWork.DrivingSessionRepository.CreateAsync(drivingSession);
                await _unitOfWork.CommitChangesAsync();

                return Result<bool>.Success(true, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException(ex.Message),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<IEnumerable<DrivingSessionDTO>>> GetUserSessions(Guid user_id, SessionFilterDTO session_filter)
        {
            var http_client = _httpClientFactory.CreateClient("UserServiceClient");
            var http_message = await http_client.PostAsJsonAsync<IEnumerable<Guid>>("api/users/ids", new List<Guid>() { user_id });

            try
            {
                http_message.EnsureSuccessStatusCode();
                var result = await http_message.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

                if (!result.Value.Any())
                {
                    return Result<IEnumerable<DrivingSessionDTO>>.Failure(ServiceError.NotFoundError($"{user_id}"), Messages.Commons.NOTFOUND);
                }

                UserDetailDTO user_info = result.Value.First() ?? throw new Exception($"{user_id}");

                if (user_info.Role != UserRole.Instructor && user_info.Role != UserRole.NoviceDriver)
                {
                    throw new Exception($"{user_info.Role}");
                }

                Expression<Func<DrivingSession, bool>> filter_expression = x =>
                    (user_info.Role == UserRole.NoviceDriver ? x.Booking.DriverId == user_info.NoviceDriver.NoviceDriverId :
                    x.Booking.InstructorId == user_info.Instructor.InstructorId)
                    && (session_filter.StartDate == null || DateOnly.FromDateTime(x.StartTime) > session_filter.StartDate)
                    && (session_filter.EndDate == null || DateOnly.FromDateTime(x.StartTime) < session_filter.EndDate)
                    && !x.IsDeleted;
                Func<IQueryable<DrivingSession>, IOrderedQueryable<DrivingSession>> order_by = x => x.OrderByDescending(u => u.StartTime);
                string included_properties = "Booking,Booking.Package,RescheduleRequests";

                IEnumerable<DrivingSession> user_driving_sessions = await _unitOfWork.DrivingSessionRepository.GetAllAsync(filter: filter_expression, orderBy: order_by, include_properties: included_properties);
                var sessions_mapped_list = user_driving_sessions.Select(x => new DrivingSessionDTO
                {
                    Id = x.Id,
                    BookingId = x.BookingId,
                    PackageName = x.Booking?.Package?.Name ?? "", // Default to empty string
                    Latitude = x.StartingLatitude,
                    Longtitude = x.StartingLongtitude,
                    PickupLocation = "", // How can we get the display name for location ?
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Status = x.Status,
                    StatusDisplayString = x.Status.ToString()
                });

                // TODO: Add pagination support if later required.
                return Result<IEnumerable<DrivingSessionDTO>>.Success(sessions_mapped_list, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DrivingSessionDTO>>.Failure(ServiceError.NotFoundError($"{ex.Message}"), Messages.Commons.UNHANDLED);
            }
        }
        public Task<Result<IEnumerable<DrivingSessionDTO>>> GetUserSessionsWithChangeRequest(Guid user_id)
        {
            throw new NotImplementedException();
        }
        public Task<Result<DrivingSessionDTO>> GetSessionDetail(Guid session_id)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<bool>> RescheduleSession(Guid session_id, SessionRescheduleRequestDTO rescheduleDTO)
        {
            Expression<Func<Booking, bool>> filter_expression = x => x.DrivingSessions.Any(x => x.Id == session_id);
            string included_properties = "DrivingSessions,DrivingSessions.RescheduleRequests";

            var filtered_bookings = await _unitOfWork.BookingRepository.GetAllAsync(filter: filter_expression, include_properties: included_properties);

            if (!filtered_bookings.Any())
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{session_id}"), Messages.Commons.NOTFOUND);
            }

            Booking target_booking = filtered_bookings[0]; // Get the booking that containing the target session.
            DrivingSession target_session = target_booking.DrivingSessions.FirstOrDefault(x => x.Id == session_id); // Get the target session.

            // Check if the session is available for reschedule.
            if (target_session.Status != SessionStatus.Planning)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"{target_session.Status.ToString()}"), Messages.Commons.UNHANDLED);
            }

            // Checking for time constraints (currently as least 24 hours before the session start)
            if ((target_session.StartTime - DateTime.Now).TotalHours < 24)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"Starting time: {target_session.StartTime.ToString()}"), Messages.Commons.UNHANDLED);
            }

            // Work flow based on the user role
            var user_role = await _jwtService.ExtractUserRoleFromToken(rescheduleDTO.JwtToken);

            switch (user_role)
            {
                case UserRole.NoviceDriver:
                    // Calculate the used time (in hours) of the package.
                    double used_time = target_booking.DrivingSessions.Where(x => !x.IsDeleted && x.Status != SessionStatus.Cancelled && x.Id != session_id).Sum(x => (x.EndTime - x.StartTime).TotalHours);
                    // Calculate the current avaiable duration (in hours) left of the booking.
                    double remaining_time = target_booking.DurationWhenBought - used_time;

                    if ((rescheduleDTO.NewEndTime - rescheduleDTO.NewStartTime).TotalHours >= remaining_time)
                    {
                        return Result<bool>.Failure(ServiceError.BadRequestError($"Thời gian còn lại: {remaining_time} giờ"), Messages.Commons.UNHANDLED);
                    }

                    target_session.StartTime = rescheduleDTO.NewStartTime;
                    target_session.EndTime = rescheduleDTO.NewEndTime;

                    // Remove the reschedule request if completed.
                    var reschedule_request = target_session.RescheduleRequests.FirstOrDefault(x => !x.IsDeleted);
                    if (reschedule_request != null)
                    {
                        reschedule_request.IsDeleted = true;
                    }

                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    await _unitOfWork.CommitChangesAsync();

                    break;
                case UserRole.Instructor:
                    // Instructor can only create a "request", the novice driver will be notified about this request.
                    if (target_session.RescheduleRequests != null)
                    {
                        target_session.RescheduleRequests = new List<RescheduleRequest>();
                    }

                    target_session.RescheduleRequests.Add(new RescheduleRequest
                    {
                        SessionId = session_id,
                        StartTime = rescheduleDTO.NewStartTime,
                        EndTime = rescheduleDTO.NewEndTime,
                        Side = RequestSide.Instructor,
                    });

                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    await _unitOfWork.CommitChangesAsync();

                    break;
                default:
                    return Result<bool>.Failure(ServiceError.RuleViolationError($"{user_role.ToString()}"), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }

        public async Task<Result<bool>> CancelSession(Guid session_id, SessionCancelRequestDTO cancelationDTO)
        {
            string included_properties = "Booking, RescheduleRequests";
            var target_session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(session_id, include_properties: included_properties);

            if (target_session == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{session_id}"), Messages.Commons.NOTFOUND);
            }

            // Check if the session is available for cancelation.
            if (target_session.Status == SessionStatus.Completed || target_session.Status == SessionStatus.Cancelled)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"{target_session.Status.ToString()}"), Messages.Commons.UNHANDLED);
            }

            var user_role = await _jwtService.ExtractUserRoleFromToken(cancelationDTO.JwtToken);
            switch (user_role)
            {
                case UserRole.NoviceDriver:
                    // Checking for time constraints (currently as least 12 hours before the session start) to refund extra time.
                    if ((target_session.StartTime - DateTime.Now).TotalHours < 12)
                    {
                        // Remove the time from the package according to business rule.
                        target_session.Booking.DurationWhenBought -= (target_session.StartTime - target_session.EndTime).TotalHours;
                    }

                    target_session.Status = SessionStatus.Cancelled;

                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    await _unitOfWork.CommitChangesAsync();
                    break;
                case UserRole.Instructor:
                    if ((target_session.StartTime - DateTime.Now).TotalHours < 12)
                    {
                        // Add more time as compensation when instructor cancel near start time according to business rule.
                        target_session.Booking.DurationWhenBought += (target_session.StartTime - target_session.EndTime).TotalHours * 0.5;
                    }

                    target_session.Status = SessionStatus.Cancelled;

                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    await _unitOfWork.CommitChangesAsync();
                    break;
                default:
                    return Result<bool>.Failure(ServiceError.RuleViolationError($"{user_role.ToString()}"), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true, Messages.Commons.SUCCESS);
        }

        //public async Task<Result<IEnumerable<InstructorScheduleResponseDTO>>> ScheduleInstructor(Guid instructorId)
        //{
        //    try
        //    {
        //        Expression<Func<Booking, bool>> filter = b => 
        //            b.InstructorId == instructorId && 
        //            !b.IsDeleted  );

        //        var bookings = await _unitOfWork.BookingRepository.GetAllAsync(
        //            filter: filter,
        //            include_properties: "DrivingSessions"
        //        );

        //        var busySessions = bookings
        //            .SelectMany(b => b.DrivingSessions ?? new List<DrivingSession>())
        //            .Where(ds => 
        //                !ds.IsDeleted && 
        //                ds.Status != SessionStatus.Cancelled &&
        //                ds.EndTime >= DateTime.UtcNow) 
        //            .OrderBy(ds => ds.StartTime)
        //            .ToList();

        //        var scheduleByDate = busySessions
        //            .GroupBy(ds => ds.StartTime.Date)
        //            .Select(group => new InstructorScheduleResponseDTO
        //            {
        //                Date = group.Key,
        //                BusySlots = _mapper.Map<List<InstructorScheduleDTO>>(
        //                    group.OrderBy(ds => ds.StartTime).ToList()
        //                )
        //            })
        //            .OrderBy(schedule => schedule.Date)
        //            .ToList();

        //        return Result<IEnumerable<InstructorScheduleResponseDTO>>.Success(
        //            scheduleByDate, 
        //            Messages.Commons.SUCCESS
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<IEnumerable<InstructorScheduleResponseDTO>>.Failure(
        //            ServiceError.UnhandledException(ex.Message),
        //            Messages.Commons.UNHANDLED
        //        );
        //    }
        //}

        public async Task<Result<List<DrivingSessionListDTO>>> GetDrivingSessionsByBooking(Guid bookingId, SessionStatus? status)
        {
            // Build filter expression
            Expression<Func<DrivingSession, bool>> filter = status.HasValue
                ? ds => ds.BookingId == bookingId && ds.Status == status.Value && !ds.IsDeleted
                : ds => ds.BookingId == bookingId && !ds.IsDeleted;

            // Get driving sessions (without includes for better performance)
            var sessions = await _unitOfWork.DrivingSessionRepository.GetAllAsync(
                filter: filter,
                orderBy: q => q.OrderBy(ds => ds.StartTime),
                include_properties: ""
            );

            if (!sessions.Any())
            {
                return Result<List<DrivingSessionListDTO>>.Success(new List<DrivingSessionListDTO>());
            }

            // Get booking IDs from sessions
            var bookingIds = sessions.Select(s => s.BookingId).Distinct().ToList();

            // Query bookings with related data directly from repository
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(
                filter: b => bookingIds.Contains(b.Id) && !b.IsDeleted,
                include_properties: "Package,Car"
            );

            // Create booking lookup dictionary for fast access
            var bookingDict = bookings.ToDictionary(b => b.Id);

            // Get unique instructor IDs from bookings
            var instructorIds = bookings.Select(b => b.InstructorId).Distinct().ToList();

            // Call UserService to get instructor info via SharedLibrary
            var instructorInfos = new Dictionary<Guid, string>();
            try
            {
                var instructorData = await _userService.GetBatchInstructorInfo(instructorIds);
                instructorInfos = instructorData.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Fullname
                );
            }
            catch (Exception ex)
            {
                // Log error but continue with empty instructor info
                Console.WriteLine($"Error fetching instructor info: {ex.Message}");
            }

            // Map sessions to DTOs using booking dictionary
            var sessionDTOs = sessions
                .Where(session => bookingDict.ContainsKey(session.BookingId))
                .Select(session =>
                {
                    var booking = bookingDict[session.BookingId];
                    var duration = (session.EndTime - session.StartTime).TotalHours;
                    var instructorName = instructorInfos.TryGetValue(booking.InstructorId, out var name)
                        ? name
                        : "Unknown";

                    return new DrivingSessionListDTO
                    {
                        Id = session.Id,
                        PackageId = booking.PackageId,
                        InstructorId = booking.InstructorId,
                        InstructorName = instructorName,
                        Date = session.StartTime.ToString("yyyy-MM-dd"),
                        StartTime = session.StartTime.ToString("HH:mm"),
                        EndTime = session.EndTime.ToString("HH:mm"),
                        Duration = Math.Round(duration, 2),
                        Location = $"{session.StartingLatitude},{session.StartingLongtitude}",
                        VehicleId = booking.CarId,
                        VehicleName = booking.Car?.Name,
                        Status = session.Status,
                        CreatedAt = session.CreatedAt
                    };
                }).ToList();

            return Result<List<DrivingSessionListDTO>>.Success(sessionDTOs);
        }

        public async Task<Result<List<SessionRouteDTO>>> CreateSessionRoutes(Guid sessionId, List<SessionRouteCreateDTO> routes)
        {
            try
            {
                // Validate session exists
                var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(sessionId);
                if (session == null)
                {
                    return Result<List<SessionRouteDTO>>.Failure(
                        ServiceError.NotFoundError($"DrivingSession {sessionId}"),
                        Messages.Commons.NOTFOUND);
                }

                // Validate routes list is not empty
                if (routes == null || !routes.Any())
                {
                    return Result<List<SessionRouteDTO>>.Failure(
                        ServiceError.BadRequestError("Routes list cannot be empty"),
                        "Danh sách tuyến đường không được để trống");
                }

                // Create SessionRoute entities
                var sessionRoutes = routes.Select(r => new SessionRoute
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    TextInstruction = r.TextInstruction,
                    StreetName = r.StreetName,
                    LatitudeStart = r.LatitudeStart,
                    LongitudeStart = r.LongitudeStart,
                }).ToList();

                // Update session status to Upcoming after routes are created
                if (session.Status == SessionStatus.Planning)
                {
                    session.Status = SessionStatus.Planning;
                    session.LastModifiedAt = DateTime.UtcNow;
                    _unitOfWork.DrivingSessionRepository.Update(session);
                }

                // Save to database
                await _unitOfWork.SessionRouteRepository.CreateMultipleAsync(sessionRoutes);
                await _unitOfWork.CommitChangesAsync();

                // Map to DTOs for response
                var routeDTOs = sessionRoutes.Select(sr => new SessionRouteDTO
                {
                    Id = sr.Id,
                    SessionId = sr.SessionId,
                    TextInstruction = sr.TextInstruction,
                    StreetName = sr.StreetName,
                    LatitudeStart = sr.LatitudeStart,
                    LongitudeStart = sr.LongitudeStart,
                }).ToList();

                return Result<List<SessionRouteDTO>>.Success(routeDTOs, Messages.Commons.SUCCESS);
            }
            catch (Exception ex)
            {
                return Result<List<SessionRouteDTO>>.Failure(
                    ServiceError.UnhandledException(ex.Message),
                    Messages.Commons.UNHANDLED);
            }
        }

        public async Task<Result<bool>> UpdateSessionStatus(Guid sessionId, SessionStatus updateStatusDTO)
        {

                var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(sessionId);
                session.Status = updateStatusDTO;
                switch (updateStatusDTO)
                {
                    case SessionStatus.InProgress:
                        session.ActualStart = DateTime.UtcNow;
                        break;
                    case SessionStatus.Completed:
                        session.ActualEnd = DateTime.UtcNow;
                        break;
                }

                _unitOfWork.DrivingSessionRepository.Update(session);
                await _unitOfWork.CommitChangesAsync();

                return Result<bool>.Success(true);
           
        }

        public async Task<Result<SessionRouteResponseDTO>> GetSessionRoutesBySessionId(Guid sessionId)
        {
            try
            {
                // Check if session exists
                var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(sessionId);
                if (session == null)
                {
                    return Result<SessionRouteResponseDTO>.Failure(
                        ServiceError.NotFoundError($"DrivingSession {sessionId}"),
                        "Không tìm thấy buổi học");
                }

                // Get routes for the session
                var routes = await _unitOfWork.SessionRouteRepository
                    .GetRoutesBySessionIdAsync(sessionId);

                // Map to DTOs
                var routeDTOs = routes.Select(r => new SessionRouteDTO
                {
                    Id = r.Id,
                    SessionId = r.SessionId,
                    TextInstruction = r.TextInstruction,
                    StreetName = r.StreetName,
                    LatitudeStart = r.LatitudeStart,
                    LongitudeStart = r.LongitudeStart
                }).ToList();

                // Create response with session's starting location and routes
                var response = new SessionRouteResponseDTO
                {
                    SessionStartingLat = session.StartingLatitude,
                    SessionStartingLong = session.StartingLongtitude,
                    Routes = routeDTOs
                };

                return Result<SessionRouteResponseDTO>.Success(response, "Lấy danh sách tuyến đường thành công");
            }
            catch (Exception ex)
            {
                return Result<SessionRouteResponseDTO>.Failure(
                    ServiceError.UnhandledException(ex.Message),
                    "Đã xảy ra lỗi khi lấy danh sách tuyến đường");
            }
        }

        public async Task<Result<SessionLogDTO>> CreateSessionLog(Guid sessionId, SessionLogCreateDTO log)
        {
            try
            {

                var sessionLog = new SessionLog
                {
                    SessionId = sessionId,
                    StreetName = log.StreetName,
                    Latitude = log.Latitude,
                    Longitude = log.Longitude,
                    Heading = log.Heading,
                    Speed = log.Speed
                };

                // Add to repository
                await _unitOfWork.SessionLogRepository.CreateAsync(sessionLog);
                await _unitOfWork.CommitChangesAsync();

                // Map to DTO
                var sessionLogDTO = new SessionLogDTO
                {
                    Id = sessionLog.Id,
                    SessionId = sessionLog.SessionId,
                    StreetName = sessionLog.StreetName,
                    Latitude = sessionLog.Latitude,
                    Longitude = sessionLog.Longitude,
                    Heading = sessionLog.Heading,
                    Speed = sessionLog.Speed,
                    CreatedAt = sessionLog.CreatedAt,
                    LastModifiedAt = sessionLog.LastModifiedAt
                };

                return Result<SessionLogDTO>.Success(sessionLogDTO, "Tạo session log thành công");
            }
            catch (Exception ex)
            {
                return Result<SessionLogDTO>.Failure(
                    ServiceError.UnhandledException(ex.Message),
                    "Đã xảy ra lỗi khi tạo session log");
            }
        }
    }
}