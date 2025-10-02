using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class LicenseCategory : BaseEntites
    {
        public string? Name {  get; set; }
        public int Priority { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }

        // relationship navigation
        public virtual ICollection<Car>? Cars{ get; set; }

    }
}
