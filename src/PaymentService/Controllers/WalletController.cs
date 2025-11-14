using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.ServiceResult;


namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("check-payment")]
        public async Task<IActionResult> CheckWallet([FromBody] PaymentRequest paymentRequest)
        {
            var query = new IsEnoughPaymentQuery
            {
                UserId = paymentRequest.UserId,
                Amount = paymentRequest.Amount,
                BookingId = paymentRequest.BookingId,
                DrivingSessionId = paymentRequest.DrivingSessionId
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("balance")]
        public async Task<IActionResult> AddBalanceToWallet([FromBody] WalletBalanceDTO wallet_balance)
        {
            var result = await _mediator.Send(new UpdateWalletBalanceCommand
            {
                UserId= wallet_balance.UserId,
                BalanceAmount = wallet_balance.Balance
            });

            return result.ToActionResult();
        }
    }
}
