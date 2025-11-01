using Microsoft.AspNetCore.Mvc;

namespace BookingService.Application.Commons.DTOs.Cars.Update
{
    public class CarUpdateDTO
    {
        [FromForm(Name = "license_plate")]
        public string LicensePlate { get; set; }

        [FromForm(Name = "brand")]
        public string BrandName { get; set; }

        [FromForm(Name = "model")]
        public string Model { get; set; }

        [FromForm(Name = "year")]
        public int Year { get; set; }

        [FromForm(Name = "color")]
        public string Color { get; set; }

        [FromForm(Name = "seats")]
        public int Seats { get; set; }

        [FromForm(Name = "fuel")]
        public string FuelType { get; set; }

        [FromForm(Name = "thumbnail_url")]
        public IFormFile ThumbnailImage { get; set; }
    }
}
