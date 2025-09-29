using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class Car : BaseEntites
    {
        // Properties
        public string Thumbnail {  get; set; }
        public string  Name{ get; set; }
        public string Description { get; set; }
        public string Insurance { get; set; }
        public string Fuel { get; set; }
        public DateOnly InsuranceEndTime { get; set; }
        public string VehicleRegistration { get; set; } = string.Empty;
        public CarStatus Status { get; set; }
        public int Seat {  get; set; }
        public string CartType { get; set; } = string.Empty;
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }
        // Key for relationship
        public Guid LicenseCategoryId { get; set; }
        public Guid InstructorId { get; set; }
        public  Guid ManufacturerId { get; set; }
        // Relationship navigation 
        public virtual LicenseCategory? LicenseCategory { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual Instructor? Instructor { get; set; }

        public virtual ICollection<CarImage>? CarImages { get; set; }

    }
}
