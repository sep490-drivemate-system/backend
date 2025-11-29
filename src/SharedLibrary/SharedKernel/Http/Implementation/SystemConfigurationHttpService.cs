using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SharedLibrary.SharedKernel.Http.DTOs.Configurations;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace SharedLibrary.SharedKernel.Http.Implementation
{
    public class SystemConfigurationHttpService(HttpService httpService, IConfiguration configuration): ISystemConfigurationHttpService
    {
        private readonly HttpService _httpService = httpService;
        private readonly IConfiguration _configuration = configuration;

        private IEnumerable<SystemConfigurationDTO>? _systemConfigs;

        public object? ConvertValueToObjectType(SystemConfigurationDTO config)
        {
            switch (config.ValueType)
            {
                case "string":
                    return config.Value;
                case "time":
                    if (!TimeOnly.TryParse(config.Value, out var timeValue))
                    {
                        return timeValue;
                    }
                    break;
                case "date":
                    if (!DateOnly.TryParse(config.Value, out var dateValue))
                    {
                        return dateValue;
                    }
                    break;
                case "datetime":
                    if (!DateTime.TryParse(config.Value, out var datetimeValue))
                    {
                        return datetimeValue;
                    }
                    break;
                case "integer":
                    if (int.TryParse(config.Value, out var integerValue))
                    {
                        return integerValue;   
                    }
                    break;
                case "decimal":
                    if (Decimal.TryParse(config.Value, out var decimalValue))
                    {
                        return decimalValue;
                    }
                    break;
                case "guid":
                    if (Guid.TryParse(config.Value, out var guidValue))
                    {
                        return guidValue;
                    }
                    break;
                default: // Not a supported value type
                    break;
            }

            return null;
        }

        // This can be used to load to cache instead!
        public async Task<IEnumerable<SystemConfigurationDTO>?> GetAllSystemConfiguration()
        {
            if (_systemConfigs == null)
            {
                string url = $"{_configuration.GetConnectionString("Userservice_connection")}/api/configurations";
                _systemConfigs = await _httpService.GetAsync<IEnumerable<SystemConfigurationDTO>>(url);
            }
            return _systemConfigs;
        }

        public async Task<SystemConfigurationDTO?> GetSystemConfiguration(string name)
        {
            if (_systemConfigs == null)
            {
                string url = $"{_configuration.GetConnectionString("Userservice_connection")}/api/configurations";
                _systemConfigs = await _httpService.GetAsync<IEnumerable<SystemConfigurationDTO>>(url);
            }

            return _systemConfigs.FirstOrDefault(x => x.Name == name);
        }
    }
}
