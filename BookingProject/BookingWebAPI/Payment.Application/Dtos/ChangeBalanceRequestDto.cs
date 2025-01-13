using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Dtos
{
    public record ChangeBalanceRequestDto
    {
        public string UserId { get; init; }
        public decimal Amount { get; init; }
        public string CurrencyName { get; init; }
    }
}
