using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class Manufacturer : BaseEntites
    {
        // Properties
        public string Name { get; set; }
        public DateTime UpdateAt {  get; set; }
        public bool IsDeleted { get; set; }

        // Relationship navigation 
        public virtual ICollection<Car>? Cars { get; set; } 
            

    }
}
