using Contracts.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class UserBalanceInfo
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public List<CurrencyDto> Currencies { get; init; }
    }
}
