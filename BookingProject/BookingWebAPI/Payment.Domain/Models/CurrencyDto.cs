using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public record CurrencyDto
    {
        public string CurrenctName { get; init; }
        public decimal Amount { get; init; }

    }
}
