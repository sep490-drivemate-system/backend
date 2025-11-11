using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Pagination;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorListFilterDTO : PaginationFilter
    {
        [FromQuery(Name = "search_key")]
        public string? SearchKey { get; set; }

        //[FromQuery(Name = "min_experience")]
        //public int? Experience { get; set; } = 0;

        //[FromQuery(Name = "min_rating")]
        //public decimal? MinRating { get; set; }

        //[FromQuery(Name = "has_license_tier")]
        //public DrivingLicenseTier? DrivingLicenseTier { get; set; }
    }
}
