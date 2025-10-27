using System;

namespace SharedLibrary.SharedKernel.Http.DTOs.Package
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int TypeRental { get; set; }
        public Guid PackageTypeId { get; set; }
        public Guid InstructorId { get; set; }
        public Guid CarId { get; set; }
        public string PackageTypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
