using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Pagination;
using UserService.Application.Commons.DTOs.Cars;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorListFilterDTO : PaginationFilter
    {
        [FromQuery(Name = "searchKey")]
        public string? SearchKey { get; set; }
        //[FromQuery(Name = "experience")]
        //public ExperienceRange? Experience { get; set; }

        //[FromQuery(Name = "minRating")]
        //public decimal? MinRating { get; set; }

    }

    public enum ExperienceRange
    {
        All,
        OneToThree,
        ThreeToFive,
        FiveToTen,
        TenPlus
    }
}
