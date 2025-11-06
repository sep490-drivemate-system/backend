using BookingService.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.DrivingSessions
{
    public class SessionFilterDTO
    {
        [FromQuery(Name = "page")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "page_size")]
        public int PageSize { get; set;} = 15;

        [FromQuery(Name="from")]
        public DateOnly? StartDate { get; set; }

        [FromQuery(Name="to")]        
        public DateOnly? EndDate { get; set; }

        [FromQuery(Name="status")]
        public string Status { get; set; } // status: planning, ongoing, completed, canceled
    }
}
