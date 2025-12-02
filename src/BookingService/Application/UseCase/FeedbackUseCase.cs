using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Feedbacks;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class FeedbackUseCase : IFeedbackUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        public FeedbackUseCase(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<InstructorOverviewFeedbackResponse> GetStatitic(Guid id)
        {
            return await _unitOfWork.FeedbackRepository.GetStatisticListInstructor(id);
        }

        public async Task<Dictionary<Guid, InstructorOverviewFeedbackResponse>> GetBatchStatistics(List<Guid> instructorIds)=> await _unitOfWork.FeedbackRepository.GetBatchStatistics(instructorIds);
        

        public async Task<Result<bool>> SaveFeedback(FeedbackCreationDTO feedbackCreationDTO, Guid driverId)
        {
            var feedback = _mapper.Map<Feedback>(feedbackCreationDTO);
            feedback.NoviceDriverId = driverId;
            try
            {
                await _unitOfWork.FeedbackRepository.CreateAsync(feedback);
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

        public async Task<Result<IEnumerable<InstructorFeedbackDTO>>> GetInstructorFeedbacks(Guid instructor_id)
        {
            Expression<Func<Feedback, bool>> filterExpression = x => x.InstructorId == instructor_id && !x.IsDeleted;
            var instructor_feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync(filter: filterExpression);

            // Fetching users
            var userServiceClient = _httpClientFactory.CreateClient("UserServiceClient");
            var responseMessage = await userServiceClient.PostAsJsonAsync("api/users/ids", instructor_feedbacks.Select(x => x.NoviceDriverId).Distinct());
            var users = await responseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            return Result<IEnumerable<InstructorFeedbackDTO>>.Success(instructor_feedbacks.Select(x => new InstructorFeedbackDTO
            {
                Name = users?.Value.FirstOrDefault(y => y.UserId == x.NoviceDriverId).FullName ?? "Anonymous",
                Avatar = users?.Value?.FirstOrDefault(y => y.UserId == x.NoviceDriverId)?.AvatarUrl ?? "",
                Description = x.InstructorFeedback,
                Rating = x.InstructorRating,
            }));
        }

        public async Task<Result<IEnumerable<CarFeedbackDTO>>> GetCarFeedbacks(Guid car_id)
        {
            Expression<Func<Feedback, bool>> filterExpression = x => x.InstructorId == car_id && !x.IsDeleted;
            var car_feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync(filter: filterExpression);

            // Fetching users
            var userServiceClient = _httpClientFactory.CreateClient("UserServiceClient");
            var responseMessage = await userServiceClient.PostAsJsonAsync("api/users/ids", car_feedbacks.Select(x => x.NoviceDriverId).Distinct());
            var users = await responseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            return Result<IEnumerable<CarFeedbackDTO>>.Success(car_feedbacks.Select(x => new CarFeedbackDTO
            {
                AvatarUrl = users.Value.FirstOrDefault(y => y.NoviceDriver.NoviceDriverId == x.NoviceDriverId)?.AvatarUrl ?? "",
                Username = users.Value.FirstOrDefault(y => y.NoviceDriver.NoviceDriverId == x.NoviceDriverId)?.FullName ?? "Anonymous",
                Rating = x.CarRating,
                Comment = x.CarFeedback,
                FeedbackDate = x.CreatedAt
            }));
        }
    }
}
