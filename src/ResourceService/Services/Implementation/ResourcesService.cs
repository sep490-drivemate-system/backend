using AutoMapper;
using ResourceService.Repositories;
using ResourceService.Repositories.Interfaces;
using ResourceService.Services.DTOs;
using ResourceService.Services.Interfaces;

namespace ResourceService.Services.Implementation
{
    public class ResourcesService: IResourcesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResourcesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResourceDto>> GetBlogsAsync()
        {
            var blogs = await _unitOfWork.ResourceRepository.GetBlogsAsync();
            return _mapper.Map<IEnumerable<ResourceDto>>(blogs);
        }

        public async Task<PagedResult<ResourceDto>> GetBlogsPagedAsync(int page, int pageSize)
        {
            var (blogs, totalCount) = await _unitOfWork.ResourceRepository.GetBlogsPagedAsync(page, pageSize);
            var blogDtos = _mapper.Map<IEnumerable<ResourceDto>>(blogs);

            return new PagedResult<ResourceDto>
            {
                Data = blogDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<BlogDetailDto?> GetBlogDetailAsync(Guid id)
        {
            var blog = await _unitOfWork.ResourceRepository.GetBlogDetailAsync(id);
            if (blog == null) return null;
            return _mapper.Map<BlogDetailDto>(blog);
        }
    }
}
