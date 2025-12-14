using ResourceService.Application.Commons.DTOs.Tags;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Interfaces.Services
{
    public interface ITagService
    {
        Task<Result<TagDTO>> GetTag(Guid id);
        Task<Result<IEnumerable<TagDTO>>> GetTags();
        Task<Result<TagDTO>> CreateTag(CreateTagDTO createTagDTO);
        Task<Result<TagDTO>> UpdateTag(Guid id, UpdateTagDTO updateTagDTO);
        Task<Result<bool>> DeleteTag(Guid id);
    }
}

