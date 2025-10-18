using Microsoft.AspNetCore.Mvc;

namespace UserService.Application.Commons.DTOs.Cars
{
    public class CarFilterDTO
    {
        [FromQuery(Name = "page")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "size")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "seats")]
        public string? SeatCounts { get; set; } = null;

        [FromQuery(Name = "brand")]
        public string? Manufacturer { get; set; } = null;

        [FromQuery(Name = "fuel")]
        public string? FuelType { get; set; } = null;

        [FromQuery(Name = "type")]
        public string? CarType { get; set; } = null;


        public static CarFilterDTO Default = new();
    }
}
