using SharedLibrary.SharedKernel.Enum;

namespace SharedLibrary.SharedKernel.Http.DTOs.Configurations
{
    public class SystemConfigurationDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public UnitOfMersurementEnum UnitOfMesurement { get; set; }

        public SystemConfigurationDTO() { }

        public SystemConfigurationDTO(Guid id, string name, string value, string valueType, UnitOfMersurementEnum unitOfMesurement)
        {
            Id = id;
            Name = name;
            Value = value;
            ValueType = valueType;
            UnitOfMesurement = unitOfMesurement;
        }
    }
}
