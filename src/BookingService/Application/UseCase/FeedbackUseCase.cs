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
        public FeedbackUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InstructorFeedbackDTO> GetInstructorFeedback(Guid id)
        {
            Expression<Func<Feedback, bool>> filter_expression = x => x.InstructorId == id && !x.IsDeleted;
            Func<IQueryable<Package>, IOrderedQueryable<Package>> order_expression = x => x.OrderBy(u => u.CreatedAt);
            
            var packages = await _unitOfWork.FeedbackRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: null);

            var packageDtos = _mapper.Map<List<PackageDto>>(packages);

            return Result<List<PackageDto>>.Success(packageDtos);
        }

        public async Task<InstructorOverviewFeedbackResponse> GetStatitic(Guid id)
        {
            return await _unitOfWork.FeedbackRepository.GetStatisticListInstructor(id);
        }
    }
}
