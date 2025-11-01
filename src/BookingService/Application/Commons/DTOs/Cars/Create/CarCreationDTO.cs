using Microsoft.AspNetCore.Mvc;

namespace BookingService.Application.Commons.DTOs.Cars.Create
{
    public class CarCreationDTO
    {
        [FromForm(Name="license_plate")]
        public string LicensePlate { get; set; }

        [FromForm(Name="instructor_id")]
        public Guid InstructorId { get; set; }

        [FromForm(Name="brand")]
        public string BrandName { get; set; }

        [FromForm(Name="model")]
        public string Model { get; set; }

        [FromForm(Name="year")]
        public int Year { get; set; }

        [FromForm(Name="color")]
        public string Color { get; set; }

        [FromForm(Name="seats")]
        public int Seats { get; set; }

        [FromForm(Name="fuel")]
        public string FuelType { get; set; }

        [FromForm(Name="thumbnail_url")]
        public IFormFile ThumbnailImage { get; set; }

        [FromForm(Name="front_image")]
        public IFormFile CarFrontImage { get; set; }

        [FromForm(Name="back_image")]
        public IFormFile CarBackImage { get; set; }

        [FromForm(Name="left_side_image")]
        public IFormFile CarLeftImage { get; set; }

        [FromForm(Name="right_side_image")]
        public IFormFile CarRightImage { get; set; }

        [FromForm(Name="interior_image")]
        public IFormFile InteriorImage { get; set; }

        [FromForm(Name="registration_front")]
        public IFormFile RegistrationFront { get; set; }

        [FromForm(Name="registration_back")]
        public IFormFile RegistrationBack { get; set; }

        [FromForm(Name="insurance_front")]
        public IFormFile InsuranceFront { get; set; }

        [FromForm(Name="insurance_back")]
        public IFormFile InsuranceBack { get; set; }

        [FromForm(Name="hourly_price")]
        public decimal HourlyPrice { get; set; }
    }
}
