using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Documents;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemDocumentController(ISystemDocumentUseCase useCase): ControllerBase
    {
        private ISystemDocumentUseCase _useCase = useCase;

        [HttpGet]
        public async Task<IActionResult> GetAllDocumentType()
        {
            var result = await _useCase.GetAllSystemDocument();
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewDocumentType([FromBody] SystemDocumentDTO document)
        {
            var result = await _useCase.CreateSystemDocument(document);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument([FromRoute] Guid id, [FromBody] SystemDocumentDTO document)
        {
            var result = await _useCase.UpdateSystemDocument(id, document);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] Guid id)
        {
            var result = await _useCase.DeleteSystemDocument(id);
            return result.ToActionResult();
        }
    }
}
