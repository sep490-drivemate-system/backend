using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Pagination;

namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingFilterDTO : PaginationFilter
    {
      public  BookingStatus Status {get; set;}
    }
}