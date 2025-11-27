
using Microsoft.AspNetCore.Http;

using ResourceService.Repositories;

using ResourceService.Services.Implementation;
using ResourceService.Services.Interfaces;

namespace Services
{
    public interface IServiceProviders
    {
        IResourcesService ResourcesService { get; }
        IQuizService QuizService { get; }
        IVoucherService VoucherService { get; }
    }

    public class ServiceProviders : IServiceProviders
    {
        private readonly IResourcesService _resourceService;
        private readonly IQuizService _quizService;
        private readonly IVoucherService _voucherService;

        public IResourcesService ResourcesService => _resourceService;
        public IQuizService QuizService => _quizService;
        public IVoucherService VoucherService => _voucherService;

        public ServiceProviders(IResourcesService resourceService, IQuizService quizService, IVoucherService voucherService)
        {
            _resourceService = resourceService;
            _quizService = quizService;
            _voucherService = voucherService;
        }
    }
}
