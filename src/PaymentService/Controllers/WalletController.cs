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
                Amount = paymentRequest.Amount
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllWallets()
        //{
        //    var query = new GetAllWalletsQuery();
        //    var result = await _mediator.Send(query);

        //    if (result.IsSuccess)
        //        return Ok(result);

        //    return BadRequest(result);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetWalletById(Guid id)
        //{
        //    var query = new GetWalletByIdQuery(id);
        //    var result = await _mediator.Send(query);

        //    if (result.IsSuccess)
        //        return Ok(result);

        //    return NotFound(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
        //{
        //    var result = await _mediator.Send(command);

        //    if (result.IsSuccess)
        //        return CreatedAtAction(nameof(GetWalletById), new { id = result.Data.Id }, result);

        //    return BadRequest(result);
        //}

        //[HttpPut("{id}/balance")]
        //public async Task<IActionResult> UpdateWalletBalance(Guid id, [FromBody] UpdateWalletBalanceCommand command)
        //{
        //    command.WalletId = id;
        //    var result = await _mediator.Send(command);

        //    if (result.IsSuccess)
        //        return Ok(result);

        //    return BadRequest(result);
        //}
    }
}
