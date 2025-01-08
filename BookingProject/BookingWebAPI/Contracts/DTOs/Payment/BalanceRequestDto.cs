using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Payment
{
    public record BalanceRequestDto(
        string UserId,
        decimal Price,
        string CurrencyName
    );
}
