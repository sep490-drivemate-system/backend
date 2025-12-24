using SharedLibrary.SharedKernel.Http.DTOs.Configurations;
using SharedLibrary.SharedKernel.ServiceResult;

namespace UserService.Application.Interfaces
{
    public interface ISystemConfigurationUseCase
    {
        public Task<Result<IEnumerable<SystemConfigurationDTO>>> GetSystemConfiguration();
        public Task<Result<SystemConfigurationDTO>> GetSystemConfigurationById(Guid id);
        public Task<Result<bool>> UpdateConfigurationValue(Guid id, string? value,int? number_date);
    }
}
