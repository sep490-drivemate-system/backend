using MediatR;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetTransactionsByBookingId
{
    public class GetTransactionsByBookingIdQuery : IRequest<Result<List<Transaction>>>
    {
        public Guid BookingId { get; set; }

        public GetTransactionsByBookingIdQuery(Guid bookingId)
        {
            BookingId = bookingId;
        }

        public GetTransactionsByBookingIdQuery() { }
    }
}
