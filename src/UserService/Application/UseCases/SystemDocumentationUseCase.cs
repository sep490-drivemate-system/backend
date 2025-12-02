using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Documents;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases
{
    public class SystemDocumentationUseCase: ISystemDocumentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemDocumentationUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> CreateSystemDocument(SystemDocumentDTO document)
        {
            await _unitOfWork.Repository<SystemDocument>().CreateAsync(new SystemDocument
            {
                DisplayName = document.Name,
                Items = String.Join("|", document.Items),
                Category = document.Type,
            });

            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteSystemDocument(Guid id)
        {
            var document = await _unitOfWork.Repository<SystemDocument>().GetByIdAsync(id);

            if (document == null || document.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            document.IsDeleted = true;
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<SystemDocumentViewDTO>>> GetAllSystemDocument()
        {
            var items = await _unitOfWork.Repository<SystemDocument>().GetAllAsync(filter: x => !x.IsDeleted);

            return Result<IEnumerable<SystemDocumentViewDTO>>.Success(items.Select(x => new SystemDocumentViewDTO
            {
                Id = x.Id,
                Name = x.DisplayName,
                Items = x.Items.Split("|"),
                Type = x.Category,
            }));
        }

        public async Task<Result<bool>> UpdateSystemDocument(Guid id, SystemDocumentDTO document)
        {
            var target_document = await _unitOfWork.Repository<SystemDocument>().GetByIdAsync(id);

            if (target_document == null || target_document.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            target_document.DisplayName = document.Name;
            target_document.Items = String.Join("|", document.Items);
            target_document.Category = document.Type;

            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
