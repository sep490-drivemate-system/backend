using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using SharedLibrary.SharedKernel.Http.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Implementation
{
    public class Package : IPackage
    {
        private readonly HttpService _httpService;
        private readonly IConfiguration _config;

        public Package(HttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _config = configuration;
        }

        public async Task<PackageResponse> GetInstructorPackages(Guid instructorId)
        {
            string bookingServiceUrl = _config["BOOKINGSERVICE:URL"];
            string url = $"{bookingServiceUrl}/api/package/instructor/{instructorId}";
            var result = await _httpService.GetAsync<PackageResponse>(url);
            return result ?? new PackageResponse();
        }

        public async Task<List<OverViewPackageDto>> GetOverViewPackages(Guid instructorId)
        {
            var packageResponse = await GetInstructorPackages(instructorId);
            var packages = packageResponse.Packages;

            var overViewPackages = new List<OverViewPackageDto>();

            // Group packages by TypeRental
            var groupedPackages = packages.GroupBy(p => p.TypeRental);

            foreach (var group in groupedPackages)
            {
                var typeRental = group.Key;
                var packageList = group.ToList();

                var overViewPackage = new OverViewPackageDto
                {
                    TypeRental = typeRental
                };

                switch (typeRental)
                {
                    case 1: // Instructor
                        overViewPackage.Name = "Instructor Package";
                        overViewPackage.Description = "Learning with instructor only";
                        // For Instructor type, take one price (first available)
                        var instructorPackage = packageList.FirstOrDefault();
                        if (instructorPackage != null)
                        {
                            overViewPackage.MinPrice = instructorPackage.Price;
                            overViewPackage.MaxPrice = instructorPackage.Price;
                        }
                        break;

                    case 2: // InstructorAndCar
                        overViewPackage.Name = "Instructor & Car Package";
                        overViewPackage.Description = "Learning with instructor and car provided";
                        // For InstructorAndCar type, get price range from low to high
                        if (packageList.Any())
                        {
                            overViewPackage.MinPrice = packageList.Min(p => p.Price);
                            overViewPackage.MaxPrice = packageList.Max(p => p.Price);
                        }
                        break;

                    case 3: // CarPackage (assuming this exists)
                        overViewPackage.Name = "Car Package";
                        overViewPackage.Description = "Car rental package";
                        // For CarPackage, get prices based on CarId
                        if (packageList.Any())
                        {
                            overViewPackage.MinPrice = packageList.Min(p => p.Price);
                            overViewPackage.MaxPrice = packageList.Max(p => p.Price);
                        }
                        break;

                    default:
                        overViewPackage.Name = "Other Package";
                        overViewPackage.Description = "Other package type";
                        if (packageList.Any())
                        {
                            overViewPackage.MinPrice = packageList.Min(p => p.Price);
                            overViewPackage.MaxPrice = packageList.Max(p => p.Price);
                        }
                        break;
                }

                overViewPackages.Add(overViewPackage);
            }

            return overViewPackages;
        }
    }
}
