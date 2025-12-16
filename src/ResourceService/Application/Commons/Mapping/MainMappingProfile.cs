using AutoMapper;
using ResourceService.Application.Commons.DTOs;
using ResourceService.Application.Commons.DTOs.Category;
using ResourceService.Application.Commons.DTOs.Quizzes;
using ResourceService.Application.Commons.DTOs.Tags;
using ResourceService.Application.Commons.DTOs.Vouchers;
using ResourceService.Domain.Entities;

namespace ResourceService.Application.Commons.Mapping
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile()
        {
            // Blog -> ResourceDto
            CreateMap<Blog, ResourceDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));

            // Blog -> BlogDetailDto
            CreateMap<Blog, BlogDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                // Map Content as plain string: first non-deleted BlogContent.Content or empty string
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src =>
                    src.Contents != null && src.Contents.Any(c => !c.IsDelete)
                        ? src.Contents.First(c => !c.IsDelete).Content
                        : string.Empty))
                .ForMember(dest => dest.ImageList, opt => opt.Ignore()); // Will be set in service layer

            // BlogContent -> BlogContentDto
            CreateMap<BlogContent, BlogContentDto>();

            // Category -> CategoryDto
            CreateMap<Category, CategoryDto>();

            // CreateCategoryDto -> Category
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Blogs, opt => opt.Ignore());

            // Create mappings (DTO -> Entity)
            CreateMap<BlogCreateDto, Blog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.ImageList, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.Ignore()); // Will be mapped separately

            // Quiz Attempt Mappings
            CreateMap<Question, QuizAttemptQuestionDetailDTO>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Choices, opt => opt.Ignore());

            CreateMap<Choice, QuizAttemptChoiceDetailDTO>()
                .ForMember(dest => dest.ChoiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore());

            CreateMap<Attempt, QuizAttemptDetailDTO>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.Name : string.Empty))
                .ForMember(dest => dest.QuizDescription, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.Description : string.Empty))
                .ForMember(dest => dest.QuizTag, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.Tag : string.Empty))
                .ForMember(dest => dest.QuizDuration, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.QuizDuration : 0))
                .ForMember(dest => dest.TotalQuestions, opt => opt.Ignore())
                .ForMember(dest => dest.CorrectAnswers, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Attempt, QuizAttemptResultDTO>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.TotalQuestions, opt => opt.Ignore())
                .ForMember(dest => dest.CorrectAnswers, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            CreateMap<Attempt, QuizAttemptHistoryDTO>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.Id : Guid.Empty))
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.Name : string.Empty))
                .ForMember(dest => dest.QuizTag, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.Tag : string.Empty))
                .ForMember(dest => dest.QuizDuration, opt => opt.MapFrom(src => src.Quiz != null ? src.Quiz.QuizDuration : 0))
                .ForMember(dest => dest.TotalQuestions, opt => opt.Ignore())
                .ForMember(dest => dest.CorrectAnswers, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.Ignore());

            // Voucher Mappings
            CreateMap<Voucher, VoucherDTO>();
            CreateMap<VoucherCreateDTO, Voucher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.UsedCount, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.VoucherUsages, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Voucher, VoucherUseResultDTO>()
                .ForMember(dest => dest.VoucherId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.UsageId, opt => opt.Ignore())
                .ForMember(dest => dest.DiscountAmount, opt => opt.Ignore())
                .ForMember(dest => dest.OrderAmount, opt => opt.Ignore())
                .ForMember(dest => dest.FinalAmount, opt => opt.Ignore());

            CreateMap<Post, CreateCategoryDto>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();

        }
    }
}
