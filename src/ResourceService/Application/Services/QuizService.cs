using AutoMapper;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.DTOs.Quizzes;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Constants;
using ResourceService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace ResourceService.Application.Services
{
    public class QuizService(IUnitOfWork unitOfWork, IMapper mapper): IQuizService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<bool>> CreateQuiz(QuizCreateOrUpdateDTO quiz, Guid inspectorId)
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
                InspectorId = inspectorId,
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
            await _unitOfWork.SaveChangesAsync();

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
            await _unitOfWork.SaveChangesAsync();

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
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<QuizAttemptStartResultDTO>> StartQuizAttempt(Guid quizId, Guid userId)
        {
            var quiz = await _unitOfWork.Repository<Quiz>().GetByIdAsync(quizId);

            if (quiz == null || quiz.IsDeleted)
            {
                return Result<QuizAttemptStartResultDTO>.Failure(ServiceError.NotFoundError($"{quizId}"), Messages.Commons.NOTFOUND);
            }

            var attempt = new Attempt
            {
                QuizId = quizId,
                UserId = userId
            };

            await _unitOfWork.Repository<Attempt>().CreateAsync(attempt);
            await _unitOfWork.SaveChangesAsync();

            var result = new QuizAttemptStartResultDTO
            {
                AttemptId = attempt.Id,
                StartTime = attempt.CreatedAt,
                DurationMinutes = quiz.QuizDuration
            };

            return Result<QuizAttemptStartResultDTO>.Success(result);
        }

        public async Task<Result<QuizAttemptResultDTO>> SubmitQuizAttempt(Guid attemptId, Guid userId, QuizAttemptRequestDTO attemptRequest)
        {
            const string includeProperties = "Quiz,Quiz.Questions,Quiz.Questions.Choices";
            var attempt = await _unitOfWork.Repository<Attempt>().GetByIdAsync(attemptId, include_properties: includeProperties);

            if (attempt == null || attempt.IsDeleted || attempt.UserId != userId)
            {
                return Result<QuizAttemptResultDTO>.Failure(ServiceError.NotFoundError($"{attemptId}"), Messages.Commons.NOTFOUND);
            }

            var quiz = attempt.Quiz;
            if (quiz == null || quiz.IsDeleted)
            {
                return Result<QuizAttemptResultDTO>.Failure(ServiceError.NotFoundError($"Quiz not found"), Messages.Commons.NOTFOUND);
            }

            var activeQuestions = quiz.Questions.Where(q => !q.IsDeleted).ToList();
            var submittedAnswers = attemptRequest.Answers?.ToList() ?? [];
            var submittedAnswerDict = submittedAnswers.ToDictionary(x => x.QuestionId, x => x.ChoiceId);

            var answerEntities = new List<Answer>();
            var answerSummaries = new List<QuizAttemptAnswerResultDTO>();
            var now = DateTime.Now;
            var isTimeExpired = (now - attempt.CreatedAt).TotalMinutes > quiz.QuizDuration;

            foreach (var question in activeQuestions)
            {
                if (submittedAnswerDict.TryGetValue(question.Id, out var choiceId) && choiceId.HasValue)
                {
                    var choice = question.Choices.FirstOrDefault(c => c.Id == choiceId.Value && !c.IsDeleted);
                    if (choice != null)
                    {
                        var isCorrect = choice.IsCorrect;

                        answerEntities.Add(new Answer
                        {
                            ChoiceId = choice.Id,
                            IsCorrect = isCorrect
                        });

                        answerSummaries.Add(new QuizAttemptAnswerResultDTO
                        {
                            QuestionId = question.Id,
                            ChoiceId = choice.Id,
                            IsCorrect = isCorrect
                        });
                    }
                    else
                    {
                        answerSummaries.Add(new QuizAttemptAnswerResultDTO
                        {
                            QuestionId = question.Id,
                            ChoiceId = null,
                            IsCorrect = null
                        });
                    }
                }
                else
                {
                    answerSummaries.Add(new QuizAttemptAnswerResultDTO
                    {
                        QuestionId = question.Id,
                        ChoiceId = null,
                        IsCorrect = null
                    });
                }
            }

            attempt.Answers = answerEntities;
            attempt.UpdatedAt = now;

            _unitOfWork.Repository<Attempt>().Update(attempt);
            await _unitOfWork.SaveChangesAsync();

            var totalQuestions = activeQuestions.Count;
            var correctAnswers = answerSummaries.Count(x => x.IsCorrect == true);
            var score = totalQuestions == 0 ? 0 : Math.Round((double)correctAnswers / totalQuestions * 100, 2);

            var result = _mapper.Map<QuizAttemptResultDTO>(attempt);
            result.TotalQuestions = totalQuestions;
            result.CorrectAnswers = correctAnswers;
            result.Score = score;
            result.Answers = answerSummaries;

            return Result<QuizAttemptResultDTO>.Success(result);
        }

        public async Task<Result<IEnumerable<QuizAttemptHistoryDTO>>> GetQuizAttemptHistory(Guid userId)
        {
            const string includeProperties = "Quiz,Quiz.Questions,Answers";
            Expression<Func<Attempt, bool>> filter = x => x.UserId == userId && !x.IsDeleted;
            
            var attempts = await _unitOfWork.Repository<Attempt>().GetAllAsync(
                filter: filter, 
                include_properties: includeProperties
            );

            var history = attempts
                .OrderByDescending(x => x.CreatedAt)
                .Select(attempt =>
                {
                    var quiz = attempt.Quiz;
                    var totalQuestions = quiz?.Questions?.Count(q => !q.IsDeleted) ?? 0;
                    var correctAnswers = attempt.Answers?.Count(a => a.IsCorrect) ?? 0;
                    var score = totalQuestions == 0 ? 0 : Math.Round((double)correctAnswers / totalQuestions * 100, 2);

                    var result = _mapper.Map<QuizAttemptHistoryDTO>(attempt);
                    result.TotalQuestions = totalQuestions;
                    result.CorrectAnswers = correctAnswers;
                    result.Score = score;
                    return result;
                });

            return Result<IEnumerable<QuizAttemptHistoryDTO>>.Success(history);
        }

        public async Task<Result<QuizAttemptDetailDTO>> GetQuizAttemptDetail(Guid attemptId, Guid userId)
        {
            const string includeProperties = "Quiz,Quiz.Questions,Quiz.Questions.Choices,Answers,Answers.Choice";
            var attempt = await _unitOfWork.Repository<Attempt>().GetByIdAsync(attemptId, include_properties: includeProperties);

            if (attempt == null || attempt.IsDeleted || attempt.UserId != userId)
            {
                return Result<QuizAttemptDetailDTO>.Failure(ServiceError.NotFoundError($"{attemptId}"), Messages.Commons.NOTFOUND);
            }

            var quiz = attempt.Quiz;
            if (quiz == null || quiz.IsDeleted)
            {
                return Result<QuizAttemptDetailDTO>.Failure(ServiceError.NotFoundError($"Quiz not found"), Messages.Commons.NOTFOUND);
            }

            var activeQuestions = quiz.Questions.Where(q => !q.IsDeleted).ToList();

            var questionDetails = activeQuestions.Select(question =>
            {
                var questionChoiceIds = question.Choices.Where(c => !c.IsDeleted).Select(c => c.Id).ToList();
                var selectedAnswer = attempt.Answers?.FirstOrDefault(a => questionChoiceIds.Contains(a.ChoiceId));

                var questionDto = _mapper.Map<QuizAttemptQuestionDetailDTO>(question);
                
                questionDto.Choices = question.Choices
                    .Where(c => !c.IsDeleted)
                    .Select(choice =>
                    {
                        var choiceDto = _mapper.Map<QuizAttemptChoiceDetailDTO>(choice);
                        choiceDto.IsSelected = selectedAnswer != null && selectedAnswer.ChoiceId == choice.Id;
                        return choiceDto;
                    });

                return questionDto;
            });

            var totalQuestions = activeQuestions.Count;
            var correctAnswers = attempt.Answers?.Count(a => a.IsCorrect) ?? 0;
            var score = totalQuestions == 0 ? 0 : Math.Round((double)correctAnswers / totalQuestions * 100, 2);

            var result = _mapper.Map<QuizAttemptDetailDTO>(attempt);
            result.TotalQuestions = totalQuestions;
            result.CorrectAnswers = correctAnswers;
            result.Score = score;
            result.Questions = questionDetails;

            return Result<QuizAttemptDetailDTO>.Success(result);
        }
    }
}
