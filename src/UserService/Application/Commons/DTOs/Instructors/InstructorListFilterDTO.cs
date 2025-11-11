using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Pagination;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorListFilterDTO : PaginationFilter
    {
        public string? SearchKey { get; set; }
    }
}
