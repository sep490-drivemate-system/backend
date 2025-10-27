using SharedLibrary.SharedKernel.Http.DTOs.Package;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Interfaces
{
    public interface IPackage
    {
        Task<PackageResponse> GetInstructorPackages(Guid instructorId);
        Task<List<OverViewPackageDto>> GetOverViewPackages(Guid instructorId);
    }
}
