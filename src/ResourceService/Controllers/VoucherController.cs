using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs.Vouchers;
using Services;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    [Route("api/vouchers")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IServiceProviders _services;
        private readonly IJwtService _jwtService;

        public VoucherController(IServiceProviders serviceProviders, IJwtService jwtService)
        {
            _services = serviceProviders;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVouchers([FromQuery] VoucherFilterDTO? filter)
        {
            var result = await _services.VoucherService.GetAllVouchers(filter);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateVoucher([FromBody] VoucherCreateDTO voucher)
        {
            var adminId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _services.VoucherService.CreateVoucher(voucher, adminId);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateVoucher([FromRoute] Guid id, [FromBody] VoucherUpdateDTO voucher)
        {
            var result = await _services.VoucherService.UpdateVoucher(id, voucher);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteVoucher([FromRoute] Guid id)
        {
            var result = await _services.VoucherService.DeleteVoucher(id);
            return result.ToActionResult();
        }

        [HttpPost("check")]
        //[Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> CheckVoucher([FromBody] VoucherUseRequestDTO request)
        {
            //var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var userId = new Guid("b1e28f12-f107-4d2f-9491-f51a7f57fcb4");
            var result = await _services.VoucherService.CheckVoucher(request, userId);
            return result.ToActionResult();
        }

        [HttpPost("use")]
        //[Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> UseVoucher([FromBody] VoucherUseRequestDTO request)
        {
            //var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var userId = new Guid("b1e28f12-f107-4d2f-9491-f51a7f57fcb4");
            var result = await _services.VoucherService.UseVoucher(request, userId);
            return result.ToActionResult();
        }
    }
}

