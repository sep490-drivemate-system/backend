
using Microsoft.AspNetCore.Http;

using ResourceService.Repositories;

using ResourceService.Services.Implementation;
using ResourceService.Services.Interfaces;

namespace Services
{
    public interface IServiceProviders
    {
        IResourcesService ResourcesService { get; }
    }

    public class ServiceProviders : IServiceProviders
    {
        private readonly IResourcesService _resourceService;

        public IResourcesService ResourcesService => _resourceService;

        public ServiceProviders(IResourcesService resourceService)
        {
            _resourceService = resourceService;
        }



    }
}
