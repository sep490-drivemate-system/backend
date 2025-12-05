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
        public async Task<(string,string)> CreatePayOSLink(PayOSPaymentDTO paymentDTO, string callBackURL)
        {
            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (orderCode <= 0)
            {
                orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
            }
            var payOS = new PayOS(
             _configuration["PAYOS:CLIENTID"],
             _configuration["PAYOS:APIKEY"],
             _configuration["PAYOS:CHECKSUMKEY"]);
            var cancelPayment= _configuration["PAYOS:CANCELURL"];
            var payOSItems = paymentDTO.Items.Select(i =>new ItemData(i.name, i.quantity, i.price)).ToList();
            var paymentLinkRequest = new PaymentData(
               orderCode: orderCode,
               amount: paymentDTO.UnitPrice,
               description: "Thanh toán đơn hàng",
               items: payOSItems,
               returnUrl: callBackURL,
               cancelUrl: cancelPayment
           );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return (response.checkoutUrl,orderCode.ToString());
        }
    }
}
