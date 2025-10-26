using SharedLibrary.SharedKernel.Entities;
using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class Policy: BaseEntites
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public PolicyType PolicyType { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
