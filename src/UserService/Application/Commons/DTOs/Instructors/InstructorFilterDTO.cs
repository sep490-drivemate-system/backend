using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commons.DTOs.Cars;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorFilterDTO
    {
        [FromQuery(Name = "page")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "size")]
        public int PageSize { get; set; } = 10;

        public static InstructorFilterDTO Default = new();
    }
}
