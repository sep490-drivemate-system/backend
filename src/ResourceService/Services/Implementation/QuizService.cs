using ResourceService.Repositories;
using ResourceService.Repositories.Models;
using ResourceService.Services.Commons.Constants;
using ResourceService.Services.DTOs.Quizzes;
using ResourceService.Services.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace ResourceService.Services.Implementation
{
    public class QuizService(IUnitOfWork unitOfWork): IQuizService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<bool>> CreateQuiz(QuizCreateOrUpdateDTO quiz)
        {
            // Validating quiz creation
            if (quiz.Duration <= 0)
            {
                return Result<bool>.Failure(ServiceError.RuleViolationError($"Duration: {quiz.Duration}"), Messages.Commons.UNHANDLED);
            }

            if (quiz.Questions.Count() == 0)
            {
                return Result<bool>.Failure(ServiceError.RuleViolationError($"Question count: {quiz.Questions.Count()}"), Messages.Commons.UNHANDLED);
            }

            if (quiz.Questions.Any(x => x.Choice.Count() <= 1))
            {
                return Result<bool>.Failure(ServiceError.RuleViolationError($"Choice count: {quiz.Questions.Count()}"), Messages.Commons.UNHANDLED);
            }

            // Creating quiz entity
            Quiz quizEntity = new Quiz
            {
                Name = quiz.Name,
                Description = quiz.Description,
                QuizDuration = quiz.Duration,
                Tag = quiz.Tag,
                InspectorId = Guid.Empty, // For simplicity, currently all new quiz will not have the creator id
                Questions = quiz.Questions.Select(x => new Question
                {
                    QuestionText = x.QuestionText,
                    Choices = x.Choice.Select(u => new Choice
                    {
                        ChoiceText = u.ChoiceText,
                        IsCorrect = u.IsCorrect,
                    }).ToList()
                }).ToList(),
            };

            await _unitOfWork.Repository<Quiz>().CreateAsync(quizEntity);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<QuizViewDTO>>> GetAllQuiz(string? tag = null, int duration = 0)
        {
            IEnumerable<string>? tags = tag?.Split(",") ?? null;
            Expression<Func<Quiz, bool>> filter_expression = x => !x.IsDeleted && (tags == null || tags.Contains(x.Tag)) && duration <= x.QuizDuration;
            string include_property = "Questions";

            var quiz_data = await _unitOfWork.Repository<Quiz>().GetAllAsync(filter: filter_expression, include_properties: include_property);

            return Result<IEnumerable<QuizViewDTO>>.Success(quiz_data.Select(x => new QuizViewDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Duration = x.QuizDuration,
                Tag = x.Tag,
                QuestionCount = x.Questions.Count()
            }));
        }

        public async Task<Result<QuizDetailDTO>> GetQuizDetailWithId(Guid id)
        {
            string include_property = "Questions,Questions.Choices";
            var question_detail = await _unitOfWork.Repository<Quiz>().GetByIdAsync(id, include_properties: include_property);

            if (question_detail == null || question_detail.IsDeleted)
            {
                return Result<QuizDetailDTO>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            return Result<QuizDetailDTO>.Success(new QuizDetailDTO
            {
                Id = id,
                Name = question_detail.Name,
                Description = question_detail.Description,
                Duration = question_detail.QuizDuration,
                Tag = question_detail.Tag,
                QuestionCount = question_detail.Questions.Count(),
                Questions = question_detail.Questions.Select(x => new QuestionDetailDTO
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    Choices = x.Choices.Select(x => new ChoiceDetailDTO
                    {
                        Id = x.Id,
                        ChoiceText = x.ChoiceText,
                        IsCorrect = x.IsCorrect,
                    })
                })
            });
        }

        public async Task<Result<bool>> UpdateQuiz(Guid id, QuizCreateOrUpdateDTO quiz)
        {
            var target_quiz = await _unitOfWork.Repository<Quiz>().GetByIdAsync(id);

            if (target_quiz == null || target_quiz.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            // Do some validation if you want here

            // update quiz
            target_quiz.Name = quiz.Name;
            target_quiz.Description = quiz.Description;
            target_quiz.Tag = quiz.Tag;
            target_quiz.QuizDuration = quiz.Duration;
            target_quiz.UpdatedAt = DateTime.Now;

            // Update quiz questions
            target_quiz.Questions = quiz.Questions.Select(x => new Question
            {
                Id = x.Id ?? Guid.NewGuid(),
                QuestionText = x.QuestionText,
                Choices = x.Choice.Select(u => new Choice
                {
                    Id = x.Id ?? Guid.NewGuid(),
                    ChoiceText = u.ChoiceText,
                    IsCorrect = u.IsCorrect,
                }).ToList()
            }).ToList();

            _unitOfWork.Repository<Quiz>().Update(target_quiz);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteQuiz(Guid id)
        {
            var target_quiz = await _unitOfWork.Repository<Quiz>().GetByIdAsync(id);

            if (target_quiz == null || target_quiz.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            target_quiz.IsDeleted = true;
            target_quiz.UpdatedAt = DateTime.Now;

            _unitOfWork.Repository<Quiz>().Update(target_quiz);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            return Result<bool>.Success(true);
        }
    }
}
