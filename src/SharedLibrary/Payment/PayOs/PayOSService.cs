using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using SharedLibrary.Payment.PayOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Payment.PayOs
{
    public class PayOSService(IConfiguration configuration) : IPayOSService
    {
        private readonly IConfiguration _configuration = configuration ;
        public async Task<string> CreatePayOSLink(PayOSPaymentDTO paymentDTO)
        {
            var payOS = new PayOS(
             _configuration["PAYOS:CLIENTID"],
             _configuration["PAYOS:APIKEY"],
             _configuration["PAYOS:CHECKSUMKEY"]);
            var domain = _configuration["PAYOS:RETURNURL"];
            var payOSItems = paymentDTO.Items.Select(i =>new ItemData(i.name, i.quantity, i.price)).ToList();
            var paymentLinkRequest = new PaymentData(
               orderCode: paymentDTO.OrderCode,
               amount: paymentDTO.UnitPrice,
               description: "Thanh toán đơn hàng",
               items: payOSItems,
               returnUrl: domain + "/payment-success",
               cancelUrl: domain + "/payment-cancel"
           );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return response.checkoutUrl;
        }
    }
}
