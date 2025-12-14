using AutoMapper;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.DTOs.QA;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;
using ResourceService.Domain.Repositories;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Services
{
    public class QAService(IUnitOfWork unitOfWork, IMapper mapper) : IQAService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<bool>> CreateQuestion(CreateQuestionDTO createQuestionDTO, Guid authorId)
        {
            var QA = _mapper.Map<QaQuestion>(createQuestionDTO);

            var question = new QaQuestion
            {
                Title = createQuestionDTO.Title,
                Content = createQuestionDTO.Content,
                AuthorId = authorId,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.QARepository.CreateAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<QAsDTO>> GetQuestions(FilterQADTO filter)
        {
            var page = filter?.PageNumber ?? 1;
            var pageSize = filter?.PageSize ?? 10;

            var questions = await _unitOfWork.Repository<QaQuestion>().GetAllAsync(
                filter: q =>
                    !q.IsDeleted &&
                    (string.IsNullOrEmpty(filter.Title) || q.Title.Contains(filter.Title)),
                orderBy: q => q.OrderByDescending(x => x.CreatedAt),
                disable_tracking: true);

            var paged = PaginatedList<QaQuestion>.Create(questions, page, pageSize);
            var items = paged.PageContent.Select(q => new QaItemDto
            {
                QuestionId = q.Id,
                Title = q.Title,
                AuthorId = q.AuthorId,
                CreatedAt = q.CreatedAt,
                LastModifiedAt = q.LastModifiedAt
            }).ToList();

            var result = new QAsDTO
            {
                CurrentPage = paged.CurrentPage,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                Items = items
            };

            return Result<QAsDTO>.Success(result);
        }

        public async Task<Result<bool>> UpdateQuestionStatus(Guid questionId, QaStatus status)
        {
            var ok = await _unitOfWork.QARepository.UpdateStatusAsync(
                questionId,
                status,
                reason: null,
                reviewerId: null);
            if (!ok)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{questionId}"), "Question not found");
            }
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> AnswerQuestion(AnswerQuestionDTO answerQuestionDTO, Guid authorId)
        {
            var question = await _unitOfWork.QARepository.GetByIdAsync(answerQuestionDTO.QuestionId);
            if (question == null || question.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{answerQuestionDTO.QuestionId}"), "Question not found");
            }

            var answer = new QaAnswer
            {
                Id = Guid.NewGuid(),
                QuestionId = answerQuestionDTO.QuestionId,
                AuthorId = authorId,
                Content = answerQuestionDTO.Content,
                IsAccepted = false,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.Repository<QaAnswer>().CreateAsync(answer);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }


        public async Task<Result<bool>> DeleteQuestion(Guid questionId)
        {
            var question = await _unitOfWork.QARepository.GetByIdAsync(questionId);
            question.IsDeleted = true;
            question.LastModifiedAt = DateTime.UtcNow;
             _unitOfWork.QARepository.Update(question);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}

