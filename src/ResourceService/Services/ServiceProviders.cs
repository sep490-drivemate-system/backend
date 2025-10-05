
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
        private readonly IUnitOfWork _unitOfWork;
        private IResourcesService _resourceService;

        public IResourcesService ResourcesService
        {
            get { return _resourceService ??= new ResourcesService(); }
        }



        public ServiceProviders(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }



    }
}
