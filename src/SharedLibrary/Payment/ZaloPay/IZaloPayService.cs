using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Payment.ZaloPay
{
    public interface IZaloPayService
    {
        Task<string> CreateZaloPayOrder(decimal? amount, string returnCallBack, string serviceName);
    }
}
