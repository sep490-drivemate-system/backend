using System.Collections.Generic;

namespace SharedLibrary.SharedKernel.Http.DTOs.Package
{
    public class PackageResponse
    {
        public List<PackageDto> Packages { get; set; } = new List<PackageDto>();
    }
}
