using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;

namespace BookingService.Application.UseCase
{
    public class DrivingSessionUseCase(IUnitOfWork unitOfWok, IJwtService jwtService, IEmailService emailService, IUser userService, IMapper mapper, IHttpClientFactory httpClientFactory, IPayment paymentService, ISystemConfigurationHttpService systemConfigurationService,IConfiguration configuration) : IDrivingSessionUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWok;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _email = emailService;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IPayment _paymentService = paymentService;
        private readonly ISystemConfigurationHttpService _systemConfigurationService = systemConfigurationService;
        private readonly IConfiguration _configuration = configuration;

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
            booking.Status = BookingStatus.InUse;
            DateTime startTime = drivingSessionCreationDTO.StartTime;


            DateTime endTimeUtc = startTime.AddHours(drivingSessionCreationDTO.Duration);
            var sessionId = Guid.NewGuid();
            var drivingSession = new DrivingSession
            {
                Id = sessionId,
                BookingId = drivingSessionCreationDTO.BookingId,
                StartTime = startTime,
                EndTime = endTimeUtc,
                PriceForCar = drivingSessionCreationDTO.PriceForCar,
                StartingLatitude = drivingSessionCreationDTO.StartingLatitude,
                StartingLongtitude = drivingSessionCreationDTO.StartingLongtitude,
                NoviceDriverNote = drivingSessionCreationDTO.SessionNote,
                DisplayEndLocationName = drivingSessionCreationDTO.DisplayEndLocationName,
                EndingLongtitude = drivingSessionCreationDTO.EndingLongtitude,
                EndingLatitude = drivingSessionCreationDTO.EndingLatitude,
                Status = SessionStatus.Planning,
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                DisplayStartLocationName = drivingSessionCreationDTO.DisplayStartLocationName,
                IsDeleted = false,
                ActualStart = DateTime.MinValue,
                ActualEnd = DateTime.MinValue,
                TotalDistance = 0,
                AverageSpeed = 0,
            };

            try
            {
                if (drivingSessionCreationDTO.PriceForCar.HasValue && drivingSessionCreationDTO.PriceForCar.Value > 0)
                {
                    var instructorId = booking.InstructorId;
                    var paymentResponse = await _paymentService.CheckWalletSession(
                            booking.DriverId,
                            instructorId,
                            drivingSessionCreationDTO.PriceForCar.Value,
                            drivingSessionCreationDTO.BookingId,
                            sessionId                            
                        );
                }

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

        //public async Task<Result<DrivingSessionlDTO>> GetSessionDetail(Guid session_id)
        //{
        //    string included_properties = "Booking,Booking.Package";
        //    var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(session_id, include_properties: included_properties);

        //    if (session == null || session.IsDeleted)
        //    {
        //        return Result<DrivingSessionlDTO>.Failure(
        //            ServiceError.NotFoundError($"Driving session {session_id}"),
        //            Messages.Commons.NOTFOUND);
        //    }

        //    var sessionDTO = _mapper.Map<DrivingSessionlDTO>(session);

        //    return Result<DrivingSessionlDTO>.Success(sessionDTO, Messages.Commons.SUCCESS);
        //}
        public async Task<Result<bool>> RescheduleSession(Guid session_id, SessionRescheduleRequestDTO rescheduleDTO)
        {
            Expression<Func<Booking, bool>> filter_expression = x => x.DrivingSessions.Any(x => x.Id == session_id);
            string included_properties = "DrivingSessions,DrivingSessions.RescheduleRequests,Package";

            var filtered_bookings = await _unitOfWork.BookingRepository.GetAllAsync(filter: filter_expression, include_properties: included_properties);

            if (!filtered_bookings.Any())
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{session_id}"), Messages.Commons.NOTFOUND);
            }

            Booking? target_booking = filtered_bookings.FirstOrDefault(); // Get the booking that containing the target session.

            if (target_booking == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{session_id}"), Messages.Commons.NOTFOUND);
            }

            DrivingSession target_session = target_booking.DrivingSessions.FirstOrDefault(x => x.Id == session_id); // Get the target session.

            // Check if the session is available for reschedule.
            if (target_session.Status != SessionStatus.Planning && target_session.Status != SessionStatus.Reschedule)
            {
                return Result<bool>.Success(false, "Đã quá thời hạn đổi lịch của session này");
            }

            // Checking for time constraints (currently as least 24 hours before the session start)
            var item = await _systemConfigurationService.GetSystemConfiguration("CancelationRefundableConstraint");

            double time_constraint_value = double.Parse(item.Value);

            if ((target_session.StartTime - DateTime.Now).TotalHours < time_constraint_value)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"Starting time: {target_session.StartTime.ToString()}"), "Thời gian đổi không còn nữa");
            }

            // getting users with user_ids from the found target session:
            var userServiceClient = _httpClientFactory.CreateClient("UserServiceClient");
            var responseMessage = await userServiceClient.PostAsJsonAsync("api/users/ids", new Guid[] { target_booking.InstructorId, target_booking.DriverId });
            var users = await responseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            // Send email to both the instructor and the novice driver about the schedule change request.
            Dictionary<string, string> InstructorEmailReplaceTerm = new Dictionary<string, string> {
                { "InstructorName", users.Value.ElementAt(0).FullName },
                { "DriverName", users.Value.ElementAt(1).FullName },
                { "Reason", rescheduleDTO.UserNote ?? "Không có lí do" },
                { "OldTime", target_session.StartTime.ToString("dddd, dd/MM/yyyy")},
                { "NewDate", rescheduleDTO.NewStartTime.ToString("dd/MM/yyyy") },
                { "NewTime", rescheduleDTO.NewStartTime.ToString("hh:mm") },
            };

            Dictionary<string, string> NoviceDriverReplaceTerm = new Dictionary<string, string> {
                { "Package", target_booking.Package.Name },
                { "OldTime", target_session.StartTime.ToString("dddd, dd/MM/yyyy")},
                { "NewTime", rescheduleDTO.NewStartTime.ToString("dd/MM/yyyy") },
                { "NewDate", rescheduleDTO.NewStartTime.ToString("hh:mm") }
            };

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
                        return Result<bool>.Failure(ServiceError.BadRequestError($"Thời gian còn lại: {remaining_time} giờ"), "Không đủ thời gian còn lại");
                    }

                    target_session.StartTime = rescheduleDTO.NewStartTime.ToLocalTime();
                    target_session.EndTime = rescheduleDTO.NewEndTime.ToLocalTime();
                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    
                    await _unitOfWork.CommitChangesAsync();
                    await _email.SendingEmail(users.Value.ElementAt(0).Email, NoviceDriverReplaceTerm, "[DriveMate] Thông báo thay đổi lịch hẹn." , EmailType.DriverReschedule);
                    await _email.SendingEmail(users.Value.ElementAt(1).Email, NoviceDriverReplaceTerm, "[DriveMate] Thông báo thay đổi lịch hẹn.", EmailType.DriverReschedule);

                    break;
                case UserRole.Instructor:
                    // Instructor can only create a "request", the novice driver will be notified about this request.
                    //_unitOfWork.DrivingSessionRepository.Update(target_session);
                    //await _unitOfWork.Repository<RescheduleRequest>().CreateAsync(new RescheduleRequest
                    //{
                    //    SessionId = session_id,
                    //    StartTime = rescheduleDTO.NewStartTime,
                    //    EndTime = rescheduleDTO.NewEndTime,
                    //    Side = RequestSide.Instructor,
                    //});

                    target_session.Status = SessionStatus.Reschedule;
                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    await _unitOfWork.CommitChangesAsync();
                    await _email.SendingEmail(users.Value.FirstOrDefault(x => x.Role == UserRole.NoviceDriver).Email, InstructorEmailReplaceTerm, "[DriveMate] Thông báo yêu cầu đổi lịch hẹn từ phía người hướng dẫn",  EmailType.InstructorReschedule);
                    
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

            var systemConfigurations = await _systemConfigurationService.GetAllSystemConfiguration();

            var config = await _systemConfigurationService.GetSystemConfiguration("CancelTimeConstraint");

            var refund_time = double.Parse(config.Value);

            // In case the system setting is not found, we can't finish => failure
            if (refund_time == 0)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError("time constraints config not found"), Messages.Commons.UNHANDLED);
            }

            switch (user_role)
            {
                case UserRole.NoviceDriver:
                    // Checking for time constraints to refund extra time.
                    if ((target_session.StartTime - DateTime.Now).TotalHours < refund_time)
                    {
                        // Remove the time from the package according to business rule.
                        target_session.Booking.DurationWhenBought -= (target_session.StartTime - target_session.EndTime).TotalHours;
                    }

                    target_session.Status = SessionStatus.Cancelled;

                    _unitOfWork.DrivingSessionRepository.Update(target_session);
                    await _unitOfWork.CommitChangesAsync();
                    break;
                case UserRole.Instructor:
                    if ((target_session.StartTime - DateTime.Now).TotalHours < refund_time)
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
            var sessionDTOs = sessions
                .Where(session => bookingDict.ContainsKey(session.BookingId))
                .Select(session =>
                {
                    var booking = bookingDict[session.BookingId];
                    var duration = (session.EndTime - session.StartTime).TotalHours;

                    return new DrivingSessionListDTO
                    {
                        Id = session.Id,
                        PackageId = booking.PackageId,
                        Date = session.StartTime.ToString("yyyy-MM-dd"),
                        StartTime = session.StartTime.AddHours(7).ToString("HH:mm"),
                        DisplayEndLocationName = session.DisplayEndLocationName,
                        DisplayStartLocationName = session.DisplayStartLocationName,
                        EndTime = session.EndTime.AddHours(7).ToString("HH:mm"),
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

        public async Task<Result<bool>> CreateSessionRoutes(Guid sessionId,SessionRouteCreateDTO routesDetail)
        {
            var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(sessionId);
            session.PolylineSesionRoute = routesDetail.PolylineSesionRoute;
            var sessionRoutes = _mapper.Map<List<SessionRoute>>(routesDetail.Routes);

            foreach (var route in sessionRoutes)
            {
                route.SessionId = sessionId;
            }

            await _unitOfWork.SessionRouteRepository.CreateMultipleAsync(sessionRoutes);
            await _unitOfWork.CommitChangesAsync();
            return Result<bool>.Success(true, Messages.Commons.SUCCESS);

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

                await _unitOfWork.SessionLogRepository.CreateAsync(sessionLog);

                if (log.IsCompleted || log.PolylineSesionLog != null)
                {
                    var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(sessionId);
                    if (session != null)
                    {
                        if (log.IsCompleted)
                        {
                            session.Status = SessionStatus.Completed;
                            session.ActualEnd = session.EndTime;
                            session.ActualStart = session.StartTime;
                        }

                        if (log.PolylineSesionLog != null)
                        {
                            session.PolylineSesionLog = log.PolylineSesionLog;
                        }

                        _unitOfWork.DrivingSessionRepository.Update(session);
                    }
                }

                await _unitOfWork.CommitChangesAsync();

                // Map to DTO using object initializer
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

        public async Task<Result<SessionDetailDTO>> GetSessionDetail(Guid sessionId)
        {

            string included_properties = "SessionRoutes,SessionLogs";
            var session = await _unitOfWork.DrivingSessionRepository.GetByIdAsync(sessionId, include_properties: included_properties);
            var sessionDetailDTO = _mapper.Map<SessionDetailDTO>(session);

            return Result<SessionDetailDTO>.Success(sessionDetailDTO, Messages.Commons.SUCCESS);
        }

        public async Task<Result<bool>> RejectRoute(Guid sessionId)
        {
            try
            {
                const string includeProperties = "Booking";
                var session = await _unitOfWork.DrivingSessionRepository
                    .GetByIdAsync(sessionId, include_properties: includeProperties);

                if (session == null || session.Booking == null)
                {
                    return Result<bool>.Failure(
                        ServiceError.NotFoundError($"Driving session {sessionId}"),
                        Messages.Commons.NOTFOUND);
                }

                var booking = session.Booking;
                var instructorId = booking.InstructorId;
                var driverId = booking.DriverId;

                var userServiceURL  = _configuration["USERSERVICE:URL"];
                var userServiceClient = _httpClientFactory.CreateClient();
                userServiceClient.BaseAddress = new Uri(userServiceURL);

                var responseMessage = await userServiceClient.PostAsJsonAsync(
                    "api/users/ids",
                    new Guid[] { instructorId, driverId });

                responseMessage.EnsureSuccessStatusCode();

                var usersResponse = await responseMessage.Content
                    .ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

                if (usersResponse?.Value == null || !usersResponse.Value.Any())
                {
                    return Result<bool>.Failure(
                        ServiceError.ServiceUnavailableError("UserServiceClient"),
                        Messages.Commons.UNHANDLED);
                }

                var users = usersResponse.Value.ToArray();
                var instructor = users.FirstOrDefault(u => u.UserId == instructorId) ?? users.First();
                var driver = users.FirstOrDefault(u => u.UserId == driverId) ?? users.Last();
                var replaceTerms = new Dictionary<string, string>
                {
                    { "InstructorName", instructor.FullName },
                    { "DriverName", driver.FullName },
                    { "Reason", "Học viên đã từ chối lộ trình được đề xuất cho buổi thuê này." },
                    { "OldTime", session.StartTime.ToString("dddd, dd/MM/yyyy") },
                    { "NewDate", session.StartTime.ToString("dd/MM/yyyy") },
                    { "NewTime", session.StartTime.ToString("HH:mm") }
                };

                await _email.SendingEmail(
                    instructor.Email,
                    replaceTerms,
                    "[DriveMate] Thông báo: Người lái mới từ chối lộ trình buổi học",
                    EmailType.RejectSession);

                return Result<bool>.Success(true,Messages.Session.SUSSCESENREJECTSESSION);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    ServiceError.UnhandledException(ex.Message),
                    Messages.Commons.UNHANDLED);
            }
        }
    }
}