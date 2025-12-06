using AutoMapper;
using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Features.Transactions.Queries.GetTransactionsByBookingId;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, Result<List<TransactionsDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<TransactionsDTO>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {

            var transactions = await _unitOfWork.TransactionRepository.GetAllAsync(tr  => tr.FromWalletId == request.WalletId,null);
            var transactionsDTO = _mapper.Map<List<TransactionsDTO>>(transactions);
            return Result<List<TransactionsDTO>>.Success(transactionsDTO);
        }
    }
}
