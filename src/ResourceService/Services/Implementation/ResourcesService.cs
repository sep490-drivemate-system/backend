using ResourceService.Repositories;
using ResourceService.Repositories.Interfaces;
using ResourceService.Services.Interfaces;

namespace ResourceService.Services.Implementation
{
    public class ResourcesService: IResourcesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResourcesService() => _unitOfWork ??= new UnitOfWork();
    }
}
