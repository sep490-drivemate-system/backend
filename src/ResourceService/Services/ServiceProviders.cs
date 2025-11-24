
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
    }

    public class ServiceProviders : IServiceProviders
    {
        private readonly IResourcesService _resourceService;
        private readonly IQuizService _quizService;

        public IResourcesService ResourcesService => _resourceService;
        public IQuizService QuizService => _quizService;

        public ServiceProviders(IResourcesService resourceService, IQuizService quizService)
        {
            _resourceService = resourceService;
            _quizService = quizService;
        }
    }
}
