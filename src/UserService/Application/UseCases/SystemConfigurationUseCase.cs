using SharedLibrary.SharedKernel.Http.DTOs.Configurations;
using SharedLibrary.SharedKernel.ServiceResult;
using Twilio.TwiML.Messaging;
using UserService.Application.Commons.Constants;
using UserService.Application.Interfaces;

namespace UserService.Application.UseCases
{
    public class SystemConfigurationUseCase(IUnitOfWork unitOfWork) : ISystemConfigurationUseCase
    {
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IEnumerable<SystemConfigurationDTO>>> GetSystemConfiguration()
        {
            var systemConfigurationEntries = await _unitOfWork.SystemConfigurationRepository.GetAllAsync(filter: x => !x.IsDeleted);

            return Result<IEnumerable<SystemConfigurationDTO>>.Success(systemConfigurationEntries.Select(x => new SystemConfigurationDTO
            {
                Id = x.Id,
                Name = x.Name,
                UnitOfMesurement = x.UnitOfMeasurement,
                Value = x.Value,
                ValueType = x.ValueType,
            }));
        }

        public async Task<Result<SystemConfigurationDTO>> GetSystemConfigurationById(Guid id)
        {
            var configurationEntry = await _unitOfWork.SystemConfigurationRepository.GetByIdAsync(id);

            if (configurationEntry == null || configurationEntry.IsDeleted)
            {
                return Result<SystemConfigurationDTO>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            return Result<SystemConfigurationDTO>.Success(new SystemConfigurationDTO
            {
                Id = configurationEntry.Id,
                Name = configurationEntry.Name,
                NumberDate = configurationEntry.NumberDate ?? 0,
                UnitOfMesurement = configurationEntry.UnitOfMeasurement,
                Value = configurationEntry.Value,
                ValueType = configurationEntry.ValueType,
            });
        }

        public async Task<Result<bool>> UpdateConfigurationValue(Guid id, string? value, int? number_date)
        {
            var target_entry = await _unitOfWork.SystemConfigurationRepository.GetByIdAsync(id);

            if (target_entry == null || target_entry.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Common.NotFoundError);
            }

            // Do some value validation here
            // Example (Please optimize this later):
            if (value != null)
            {
                switch (target_entry.ValueType)
                {
                    case "string":
                        // By default, the string doesn't need to be parsed or validated
                        break;
                    case "time":
                        if (!TimeOnly.TryParse(value, out var timeValue))
                        {
                            return Result<bool>.Failure(ServiceError.BadRequestError($"{id}"), Messages.Common.UnknownError);
                        }
                        break;
                    case "date":
                        if (!DateOnly.TryParse(value, out var dateValue))
                        {
                            return Result<bool>.Failure(ServiceError.BadRequestError($"{id}"), Messages.Common.UnknownError);
                        }
                        break;
                    case "datetime":
                        if (!DateTime.TryParse(value, out var datetimeValue))
                        {
                            return Result<bool>.Failure(ServiceError.BadRequestError($"{id}"), Messages.Common.UnknownError);
                        }
                        break;
                    case "integer":
                        if (!int.TryParse(value, out var integerValue))
                        {
                            return Result<bool>.Failure(ServiceError.BadRequestError($"{id}"), Messages.Common.UnknownError);
                        }
                        break;
                    case "decimal":
                        if (!Decimal.TryParse(value, out var decimalValue))
                        {
                            return Result<bool>.Failure(ServiceError.BadRequestError($"{id}"), Messages.Common.UnknownError);
                        }
                        break;
                    case "guid":
                        if (!Guid.TryParse(value, out var guidValue))
                        {
                            return Result<bool>.Failure(ServiceError.BadRequestError($"{id}"), Messages.Common.UnknownError);
                        }
                        break;
                    default: // Not a supported value type
                        return Result<bool>.Failure(ServiceError.InvalidStateError(Messages.Common.UnknownError), Messages.Common.UnknownError);
                }

                target_entry.Value = value;
            }
            
            target_entry.NumberDate = number_date ?? target_entry.NumberDate;
            _unitOfWork.SystemConfigurationRepository.Update(target_entry);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
