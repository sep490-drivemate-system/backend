using MediatR;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetInformationBank
{
    public class GetInformationBankQuery
        : IRequest<VietQrLookupResponse>
    {
        public int Bin { get; init; }
        public string AccountNumber { get; init; }
    }
}
