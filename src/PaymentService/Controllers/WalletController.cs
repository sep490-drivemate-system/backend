using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;


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

            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

      
    }
}
