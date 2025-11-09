using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enum;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class DrivingSessionUseCase(IUnitOfWork unitOfWok, IJwtService jwtService, IMapper mapper): IDrivingSessionUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWok;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IMapper _mapper = mapper;

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

            // Calculate end time based on duration
            DateTime endTime = drivingSessionCreationDTO.StartTime.AddMinutes(drivingSessionCreationDTO.Duration);

            // Create new driving session entity
            var drivingSession = new DrivingSession
            {
                Id = Guid.NewGuid(),
                BookingId = drivingSessionCreationDTO.BookingId,
                StartTime = drivingSessionCreationDTO.StartTime,
                EndTime = endTime,
                StartingLatitude = drivingSessionCreationDTO.StartingLatitude,
                StartingLongtitude = drivingSessionCreationDTO.StartingLongtitude,
                NoviceDriverNote = drivingSessionCreationDTO.SessionNote,
                Status = SessionStatus.Planned,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
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

        public Task<Result<IEnumerable<DrivingSessionDTO>>> GetUserSessions(Guid user_id, SessionFilterDTO session_filter)
        {
            throw new NotImplementedException();
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
            if (target_session.Status != SessionStatus.Planned)
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
    }
}
