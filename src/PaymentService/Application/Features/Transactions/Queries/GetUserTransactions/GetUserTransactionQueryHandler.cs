using MediatR;
using PaymentService.Application.Common.DTOs.Transaction;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace PaymentService.Application.Features.Transactions.Queries.GetUserTransactions
{
    public class GetUserTransactionQueryHandler : IRequestHandler<GetUserTransactionQuery, Result<PaginatedList<PersonalTransactionViewDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserTransactionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedList<PersonalTransactionViewDTO>>> Handle(GetUserTransactionQuery request, CancellationToken cancellationToken)
        {
            // Getting user transaction with provided id and filtering

            string[]? dateParts = string.IsNullOrEmpty(request.Filter.DateString) ? null : request.Filter.DateString.Split("/");// Preparing date part

            Expression<Func<Transaction,bool>> filterExpression = x => (x.FromWalletId == request.UserId || x.ToWalletId == request.UserId) 
            && (request.Filter.Status == 0 || x.Status == request.Filter.Status)
            && (request.Filter.Amount == 0 || x.TransactionValue == request.Filter.Amount)
            && (dateParts == null || x.CreatedAt.Month == int.Parse(dateParts[0]) && x.CreatedAt.Year == int.Parse(dateParts[1]))
            && !x.IsDelete;


            var userTransactionList = await _unitOfWork.TransactionRepository.GetAllAsync(filter: filterExpression, x => x.OrderByDescending(y => y.CreatedAt));

            var mappedTransactionList = userTransactionList.Select(x => new PersonalTransactionViewDTO
            {
                Title = x.ReferenceCode, // Will be changed later
                Value = x.TransactionValue,
                StatusText = x.Status.ToString(),
                Status = x.Status,
                Date = x.CreatedAt,
            }).ToList();

            // Handling return value
            return Result<PaginatedList<PersonalTransactionViewDTO>>.Success(PaginatedList<PersonalTransactionViewDTO>.Create(mappedTransactionList, request.Filter.Page, request.Filter.PageSize));
        }
    }
}
