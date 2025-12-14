using MediatR;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enum;
using SharedLibrary.Payment.PayOs;
using SharedLibrary.Payment.PayOs.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;
using SharedLibrary.Payment.VnPay;
using SharedLibrary.Payment.ZaloPay;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using PaymentService.Application.Common.Constants;

namespace PaymentService.Application.Features.Wallet.Queries.GetRequestDeposit
{
    public class GetRequestDepositQueryHandler : IRequestHandler<GetRequestDepositQuery, Result<string>>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayOSService _payOsService;
        private readonly IVNPayService _vnPayService;
        private readonly IZaloPayService _zaloPayService;

        public GetRequestDepositQueryHandler(
            IConfiguration configuration,
            IPayOSService payOsService,
            IVNPayService vnPayService,
            IUnitOfWork unitOfWork,
            IZaloPayService zaloPayService)
        {
            _configuration = configuration;
            _payOsService = payOsService;
            _vnPayService = vnPayService;
            _zaloPayService = zaloPayService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetRequestDepositQuery request, CancellationToken cancellationToken)
        {
            var callbackUrlMobile = _configuration["PAYMENTCALLBACK_MOBILE:URL"];
            var callbackUrlWeppapp= _configuration["PAYMENTCALLBACK_WEPAPP:URL"];
            string callbackUrl = request.Platform == Domain.Enum.ClientPlatform.WebApp ? callbackUrlWeppapp : callbackUrlMobile;

            switch (request.PaymentMethod)
            {
                case PaymentMethod.PayOs:
                    return await HandlePayOsPayment(request.Amount, request.UserId,callbackUrl);

                case PaymentMethod.VnPay:
                    return await HandleVnPayPayment(request.Amount, request.UserId, callbackUrl);

                case PaymentMethod.ZaloPay:
                    return await HandleZaloPayPayment(request.Amount, request.UserId, callbackUrl);

                default:
                    return Result<string>.Failure(
                        ServiceError.NotFoundError(Messages.Wallet.UnsupportedPaymentMethod));
            }
        }

        private async Task<Result<string>> HandlePayOsPayment(decimal amount,Guid userId,string callbackUrl)
        {
            var paymentDto = new PayOSPaymentDTO
            {
                UnitPrice = (int)amount,                
                Items = new List<ItemData>
                {
                    new ItemData(
                        name: "Wallet deposit",
                        quantity: 1,
                        price: (int)amount
                    )
                },
            };
            var (paymentUrl, referenceCode) = await _payOsService.CreatePayOSLink(paymentDto,callbackUrl);
            var transaction = new Domain.Entities.Transaction
            {
                PaymentMethod = PaymentMethod.VnPay,
                TransactionValue = amount,
                Status = PaymentStatus.Failed,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ToWalletId = userId,
                ReferenceCode = referenceCode.ToString(),
            };
            await _unitOfWork.TransactionRepository.CreateAsync(
                 transaction);

            return Result<string>.Success(paymentUrl);
        }

        private async Task<Result<string>> HandleVnPayPayment(decimal amount, Guid userId, string callbackUrl)
        {
            var (paymentUrl, referenceCode) = await _vnPayService.CreateVNPayOrder(amount, callbackUrl);
            var transaction = new Domain.Entities.Transaction
            {
                PaymentMethod = PaymentMethod.VnPay,
                TransactionValue = amount,
                Status = PaymentStatus.Failed,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ToWalletId = userId,
                ReferenceCode = referenceCode.ToString(),
            };
           await _unitOfWork.TransactionRepository.CreateAsync(
                transaction);
            return Result<string>.Success(paymentUrl);
        }

        private async Task<Result<string>> HandleZaloPayPayment(decimal amount, Guid userId, string callbackUrl)
        {
            var (paymentUrl, referenceCode) = await _zaloPayService.CreateZaloPayOrder(amount, callbackUrl);
            var transaction = new Domain.Entities.Transaction
            {
                PaymentMethod = PaymentMethod.VnPay,
                TransactionValue = amount,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ToWalletId = userId,
                Status = PaymentStatus.Failed,
                ReferenceCode = referenceCode.ToString(),
            };
            await _unitOfWork.TransactionRepository.CreateAsync(
                 transaction);
            return Result<string>.Success(paymentUrl);
        }
    }
}
