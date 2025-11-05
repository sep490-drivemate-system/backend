using SharedLibrary.SharedKernel.Entities;
using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class Policy: BaseEntites
    {
        // Properties
        public string Name { get; set; }
        public string Description { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
