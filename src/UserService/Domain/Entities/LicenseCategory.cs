using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class LicenseCategory : BaseEntites
    {
        public string? Name {  get; set; }
        public int Priority { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual ICollection<User> Users { get; set; }
    }
}
