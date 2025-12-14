using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Payment.VnPay
{
    public interface IVNPayService
    {
        Task<(string paymentUrl, Guid referenceCode)> CreateVNPayOrder(decimal amount, string returnUrl);
    }
}
