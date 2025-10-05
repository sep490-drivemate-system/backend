using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class Package : BaseEntites
    {
        // properties
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }

        // relationship navigation 
        public virtual ICollection<PackageCar> PackageCars { get; set; } = new List<PackageCar>();
    }
}
