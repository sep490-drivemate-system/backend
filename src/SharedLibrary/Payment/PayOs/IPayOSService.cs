using SharedLibrary.Payment.PayOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Payment.PayOs
{
    public interface IPayOSService
    {
        Task<(string paymentUrl, string referenceCode)> CreatePayOSLink(PayOSPaymentDTO paymentDTO,string callBackURL);
    }
}
