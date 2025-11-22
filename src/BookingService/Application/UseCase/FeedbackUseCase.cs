using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Feedbacks;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class FeedbackUseCase : IFeedbackUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeedbackUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public async Task<InstructorFeedbackDTO> GetInstructorFeedback(Guid id)
        //{
        //    Expression<Func<Feedback, bool>> filter_expression = x => x.InstructorId == id && !x.IsDeleted;
        //    Func<IQueryable<Package>, IOrderedQueryable<Package>> order_expression = x => x.OrderBy(u => u.CreatedAt);
            
        //    var packages = await _unitOfWork.FeedbackRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: null);

        //    var packageDtos = _mapper.Map<List<PackageDto>>(packages);

        //    return Result<List<PackageDto>>.Success(packageDtos);
        //}

        public async Task<InstructorOverviewFeedbackResponse> GetStatitic(Guid id)
        {
            return await _unitOfWork.FeedbackRepository.GetStatisticListInstructor(id);
        }

        public async Task<Dictionary<Guid, InstructorOverviewFeedbackResponse>> GetBatchStatistics(List<Guid> instructorIds)
        {
            return await _unitOfWork.FeedbackRepository.GetBatchStatistics(instructorIds);
        }

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
    }
}
