using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;

namespace UserService.Domain.Entities
{
    public class SystemConfiguration: BaseEntites
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int? NumberDate { get; set; }
        public UnitOfMersurementEnum UnitOfMeasurement { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
