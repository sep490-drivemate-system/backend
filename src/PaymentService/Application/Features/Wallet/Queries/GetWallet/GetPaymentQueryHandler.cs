using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using WalletEntity = PaymentService.Domain.Entities.Wallet;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.GetWallet
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, Result<decimal>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaymentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<decimal>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync<Guid>(request.WalletId);

            if (wallet == null)
            {
                var createWalletDto = new CreateWalletDto { InitialBalance = 0m };
                wallet = _mapper.Map<WalletEntity>(createWalletDto);
                wallet.Id = request.WalletId;

                await _unitOfWork.WalletRepository.CreateAsync(wallet);
                await _unitOfWork.SaveChangesAsync();
            }
            return Result<decimal>.Success(wallet.Balance);
        }
    }
}
