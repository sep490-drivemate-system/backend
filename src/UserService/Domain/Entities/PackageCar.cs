using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class PackageCar : BaseEntites
    {
        // properties 
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }
        public int Price { get; set; }
        // key for relationship
        public Guid PackageId { get; set; }
        public Guid CarId { get; set; }
        // relationship navigation 
        public virtual Package? Package { get; set; }
        public virtual Car? Car { get; set; }
    }
}
