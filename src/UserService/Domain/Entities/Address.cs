using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class Address : BaseEntites
    {
        // Properties
        public string Location {  get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Key for relationship
        public Guid UserId { get; set; }

        // Relationship navigation
        public virtual User? User { get; set; }
    }
}
