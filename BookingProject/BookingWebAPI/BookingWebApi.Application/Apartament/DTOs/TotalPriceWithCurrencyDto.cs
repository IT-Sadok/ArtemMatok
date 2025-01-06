using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Apartament.DTOs
{
    public record TotalPriceWithCurrencyDto(
      decimal TotalPrice,
      string CurrencyName
    );
}
