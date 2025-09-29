using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class CarImage : BaseEntites
    {
        // Properties
        public string? ImageUrl { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }

        // key for relationship
        public Guid CarId { get; set; }

        // Relationship navigation
        public virtual Car? Car { get; set; }
    }
}
