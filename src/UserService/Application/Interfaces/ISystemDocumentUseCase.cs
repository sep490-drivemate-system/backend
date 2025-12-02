using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Documents;

namespace UserService.Application.Interfaces
{
    public interface ISystemDocumentUseCase
    {
        Task<Result<IEnumerable<SystemDocumentViewDTO>>> GetAllSystemDocument();
        Task<Result<bool>> CreateSystemDocument(SystemDocumentDTO document);
        Task<Result<bool>> UpdateSystemDocument(Guid id, SystemDocumentDTO document);
        Task<Result<bool>> DeleteSystemDocument(Guid id);
    }
}
