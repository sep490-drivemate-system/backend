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
    }
}
