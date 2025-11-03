using Microsoft.AspNetCore.Mvc;

namespace BookingService.Application.Commons.DTOs.Cars.Get
{
    public class CarListFilterDTO
    {
        [FromQuery(Name = "page")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "size")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "seats")]
        public int? SeatCounts { get; set; } = null;

        [FromQuery(Name = "brand")]
        public string? Manufacturer { get; set; } = null;

        [FromQuery(Name = "fuel")]
        public string? FuelType { get; set; } = null;

        [FromQuery(Name = "type")]
        public string? CarType { get; set; } = null;

        [FromQuery(Name = "order_by")]
        public string? OrderBy { get; set; } = null;


    }
}
