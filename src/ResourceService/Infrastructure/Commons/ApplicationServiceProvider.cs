using ResourceService.Application.Interfaces;
using ResourceService.Application.Interfaces.Services;

namespace ResourceService.Infrastructure.Commons
{
    public class ApplicationServiceProvider: IApplicationServiceProvider
    {
        private readonly IResourcesService _resourceService;
        private readonly IQuizService _quizService;
        private readonly IVoucherService _voucherService;
        private readonly IQAService _qAService;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;


        public IResourcesService ResourcesService => _resourceService;
        public IQuizService QuizService => _quizService;
        public IVoucherService VoucherService => _voucherService;
        public IQAService QAService => _qAService;
        public IPostService PostService => _postService;
        public ITagService TagService => _tagService;
        public ICategoryService CategoryService => _categoryService;

        public ApplicationServiceProvider(
            IResourcesService resourceService, 
            IQuizService quizService, 
            IVoucherService voucherService, 
            IPostService postService,
            IQAService qAService,
            ITagService tagService,
            ICategoryService categoryService)
        {
            _resourceService = resourceService;
            _quizService = quizService;
            _voucherService = voucherService;
            _postService = postService;
            _qAService = qAService;
            _tagService = tagService;
            _categoryService = categoryService;
        }
    }
}
