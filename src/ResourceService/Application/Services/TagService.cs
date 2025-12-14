using AutoMapper;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.DTOs.Tags;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Constants;
using ResourceService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Services
{
    public class TagService(IUnitOfWork unitOfWork, IMapper mapper) : ITagService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<Result<TagDTO>> GetTag(Guid id)
        {
            var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(id);
            if (tag == null || tag.IsDeleted)
            {
                return Result<TagDTO>.Failure(ServiceError.NotFoundError(Messages.Tag.TAG_ALREADY_EXISTS));
            }
            var tagDTO = _mapper.Map<TagDTO>(tag);
            return Result<TagDTO>.Success(tagDTO);
        }

        public async Task<Result<IEnumerable<TagDTO>>> GetTags()
        {
            var tags = await _unitOfWork.Repository<Tag>().GetAllAsync(
                filter: t => !t.IsDeleted);

            var dtos = tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug
            });

            return Result<IEnumerable<TagDTO>>.Success(dtos);
        }

        public async Task<Result<TagDTO>> CreateTag(CreateTagDTO createTagDTO)
        {
            var existingTag = await _unitOfWork.Repository<Tag>().GetAllAsync(
                filter: t => t.Slug == createTagDTO.Slug && !t.IsDeleted);

            if (existingTag.Any())
            {
                return Result<TagDTO>.Failure(ServiceError.ConflictError(Messages.Tag.SLUG_ALREADY_EXISTS));
            }

            var tag = _mapper.Map<Tag>(createTagDTO);

            await _unitOfWork.Repository<Tag>().CreateAsync(tag);
            await _unitOfWork.SaveChangesAsync();
            var tagDTO = _mapper.Map<TagDTO>(tag);

            return Result<TagDTO>.Success(tagDTO);
        }

        public async Task<Result<TagDTO>> UpdateTag(Guid id, UpdateTagDTO updateTagDTO)
        {
            var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(updateTagDTO.Slug))
            {
                var existingTag = await _unitOfWork.Repository<Tag>().GetAllAsync(
                    filter: t => t.Slug == updateTagDTO.Slug && t.Id != id && !t.IsDeleted);

                if (existingTag.Any())
                {
                    return Result<TagDTO>.Failure(ServiceError.ConflictError(Messages.Tag.SLUG_ALREADY_EXISTS));
                }

                tag.Slug = updateTagDTO.Slug;
            }

            tag.LastModifiedAt = DateTime.Now;
            _unitOfWork.Repository<Tag>().Update(tag);
            await _unitOfWork.SaveChangesAsync();
            var tagDTO = _mapper.Map<TagDTO>(tag);
            return Result<TagDTO>.Success(tagDTO);
        }

        public async Task<Result<bool>> DeleteTag(Guid id)
        {
            var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(id);
            tag.IsDeleted = true;
            tag.LastModifiedAt = DateTime.Now;
            _unitOfWork.Repository<Tag>().Update(tag);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}

