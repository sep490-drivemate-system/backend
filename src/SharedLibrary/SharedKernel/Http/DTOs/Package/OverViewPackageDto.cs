using System;

namespace SharedLibrary.SharedKernel.Http.DTOs.Package
{
    public class OverViewPackageDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int TypeRental { get; set; }
    }
}
