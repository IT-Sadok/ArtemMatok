using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Validators
{
    public static class CurrencyValidator
    {
        private static readonly HashSet<string> ValidCurrencies = new HashSet<string>()
        {
            "USD", "EUR", "UAH"
        };

        public static bool IsValidCurrency(string currencyName)
        {
            var currency = currencyName.ToUpper();

            return ValidCurrencies.Contains(currencyName);
        }
    }
}
