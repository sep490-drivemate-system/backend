using CloudinaryDotNet;
using MediatR;
using PaymentService.Application.Common.Constants;
using PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Collections;
using System.Text.Json;

namespace PaymentService.Application.Features.Wallet.Commands.Deposit
{
    public class DepositCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DepositCommand, Result<decimal?>>
    {
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<decimal?>> Handle(DepositCommand depositCommand, CancellationToken cancellationToken)
        {

                if (depositCommand.Data.ContainsKey("code"))
                {
                    var payOsesult = await HandlePayOsCallback(depositCommand.Data);
                    return Result<decimal?>.Success(payOsesult);
                }
                if (depositCommand.Data.ContainsKey("vnp_BankCode"))
                {
                    var vnPayResult = await HandleVnPayCallback(depositCommand.Data);
                return Result<decimal?>.Success(vnPayResult); 
                }
                if (
                    depositCommand.Data.ContainsKey("bankcode")
                )
                {
                    var zaloPayResult = await HandleZaloPayCallback(depositCommand.Data);
                return Result<decimal?>.Success(zaloPayResult); ;
                }
            return Result<decimal?>.Failure(
                ServiceError.NotFoundError(Messages.Wallet.UnHandlePayment));
            
        }
        #region PAYOS
        private async Task<decimal?> HandlePayOsCallback(
            IQueryCollection data
        )
        {
            var code = data["code"].ToString();

            var status = code == "00" ;
            if (status)
            {
                var orderCode = data["orderCode"].ToString();
                var transaction = await _unitOfWork.TransactionRepository.GetByReferenceCodeAsync(orderCode);
                 transaction.Status = Domain.Enum.PaymentStatus.Completed;
                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
                await _unitOfWork.CommitAsync();
                return transaction.TransactionValue;
            }      
            return null;
        }

      

        #endregion

        #region VNPAY
        private async Task<decimal?> HandleVnPayCallback(
            IQueryCollection data)
        {
            bool status = data["vnp_ResponseCode"] == "00";
            var vnp_TxnRef = data["vnp_TxnRef"].ToString();
            var vnp_SecureHash = data["vnp_SecureHash"].ToString();
            if(status)
            {
                var referenceCode = vnp_TxnRef;
                var transaction = await _unitOfWork.TransactionRepository.GetByReferenceCodeAsync(referenceCode);
                transaction.Status = Domain.Enum.PaymentStatus.Completed;
                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
                await _unitOfWork.CommitAsync();
                return transaction.TransactionValue;
            }           
            return null;
        }
        #endregion

        #region ZALOPAY
        private async Task<decimal?> HandleZaloPayCallback(
            IQueryCollection data
        )
        {
            bool status = data["status"] == "1";
            var checksumZaloPay = data["apptransid"].ToString();
            if (status)
            {
                var referenceCode = checksumZaloPay;
                var transaction = await _unitOfWork.TransactionRepository.GetByReferenceCodeAsync(referenceCode);
                transaction.Status = Domain.Enum.PaymentStatus.Completed;
                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
                await _unitOfWork.CommitAsync();
                return transaction.TransactionValue;
            }
            return null;
        }
        #endregion
    }
}
