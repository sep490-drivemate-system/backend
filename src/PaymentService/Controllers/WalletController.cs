using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace PaymentService.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class WalletController : ControllerBase
    //{
    //    private readonly IMediator _mediator;

    //    public WalletController(IMediator mediator)
    //    {
    //        _mediator = mediator;
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> GetAllWallets()
    //    {
    //        var query = new GetAllWalletsQuery();
    //        var result = await _mediator.Send(query);
            
    //        if (result.IsSuccess)
    //            return Ok(result);
            
    //        return BadRequest(result);
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetWalletById(Guid id)
    //    {
    //        var query = new GetWalletByIdQuery(id);
    //        var result = await _mediator.Send(query);
            
    //        if (result.IsSuccess)
    //            return Ok(result);
            
    //        return NotFound(result);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
    //    {
    //        var result = await _mediator.Send(command);
            
    //        if (result.IsSuccess)
    //            return CreatedAtAction(nameof(GetWalletById), new { id = result.Data.Id }, result);
            
    //        return BadRequest(result);
    //    }

    //    [HttpPut("{id}/balance")]
    //    public async Task<IActionResult> UpdateWalletBalance(Guid id, [FromBody] UpdateWalletBalanceCommand command)
    //    {
    //        command.WalletId = id;
    //        var result = await _mediator.Send(command);
            
    //        if (result.IsSuccess)
    //            return Ok(result);
            
    //        return BadRequest(result);
    //    }
   // }
}
