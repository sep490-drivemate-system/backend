using ResourceService.Application.Interfaces;
using ResourceService.Application.Interfaces.Services;

namespace ResourceService.Infrastructure.Commons
{
    public class ApplicationServiceProvider: IApplicationServiceProvider
    {
        private readonly IResourcesService _resourceService;
        private readonly IQuizService _quizService;
        private readonly IVoucherService _voucherService;

        public IResourcesService ResourcesService => _resourceService;
        public IQuizService QuizService => _quizService;
        public IVoucherService VoucherService => _voucherService;

        public ApplicationServiceProvider(IResourcesService resourceService, IQuizService quizService, IVoucherService voucherService)
        {
            _resourceService = resourceService;
            _quizService = quizService;
            _voucherService = voucherService;
        }
    }
}
