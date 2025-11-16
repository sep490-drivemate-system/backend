using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.DTOs.Wallet
{
    public class WalletBalanceDTO
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
