using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;

namespace BookingService.Application.Commons.DTOs.Cars.Create
{
    public class CarCreationDTO
    {
        public Guid InstructorId { get; set; }
        public string Description { get; set; }
        public decimal HourlyPrice { get; set; }

        // Car images
        public IFormFile ThumbnailImage { get; set; }
        public IFormFile CarFrontImage { get; set; }
        public IFormFile CarBackImage { get; set; }
        public IFormFile CarLeftImage { get; set; }
        public IFormFile CarRightImage { get; set; }
        public IFormFile InteriorImage { get; set; }

        // Registration
        public IFormFile RegistrationFront { get; set; }
        public IFormFile RegistrationBack { get; set; }
        public DrivingLicenseTier LicenseTier { get; set; }
        public string LicensePlate { get; set; }
        public Guid BrandId { get; set; }
        public string Model { get; set; }
        public string CarType { get; set; }
        public int Year { get; set; } // Not Used
        public string Color { get; set; } // Not Used
        public int Seats { get; set; }
        public string FuelType { get; set; }

        // Insurance
        public IFormFile InsuranceFront { get; set; }
        public IFormFile InsuranceBack { get; set; }
        public DateOnly InsuranceEndTime { get; set; }
    }
}
