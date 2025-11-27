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
    }
}

