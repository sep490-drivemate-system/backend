using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class SavedLocation : BaseEntites
    {
        // Properties
        public string DisplayName {  get; set; }
        public float LocationLatitude { get; set; }
        public float LocationLongtitude { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Key for relationship
        public Guid NoviceDriverId { get; set; }

        // Relationship navigation
        public virtual NoviceDriver? NoviceDriver { get; set; }
    }
}
