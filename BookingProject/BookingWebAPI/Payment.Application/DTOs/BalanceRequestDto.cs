using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.DTOs
{
    public record BalanceRequestDto
    {
        public string UserId { get; init; }
        public decimal Price { get; init; }
        public string CurrencyName { get; init; }

        public BalanceRequestDto() { }
    }

}
