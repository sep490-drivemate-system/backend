using SharedLibrary.SharedKernel.Http.DTOs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Interfaces
{
    public interface ISystemConfigurationHttpService
    {
        Task<IEnumerable<SystemConfigurationDTO>?> GetAllSystemConfiguration();

        Task<SystemConfigurationDTO?> GetSystemConfiguration(string name);

        object? ConvertValueToObjectType(SystemConfigurationDTO config);
    }
}
