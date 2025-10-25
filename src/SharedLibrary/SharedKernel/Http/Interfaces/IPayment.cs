using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Interfaces
{
    public interface IPayment
    {
        Task<PaymentResponse> CheckWalletBooking(Guid userId,decimal amount);
    }
}
