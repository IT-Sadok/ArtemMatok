using Payment.Domain.Models;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories.PaymentRepository
{
    public class PaymentRepository(PaymentDbContext _context) : IPaymentRepository
    {
        public async Task<bool> CreateBalanceAsync(string userId)
        {
            var balance = new UserBalance
            {
                UserId = userId,
                Currencies = new List<Currency>
                {
                    new Currency
                    {
                        Amount = 0,
                        CurrencyName = "USD",
                        UserBalanceId = userId
                    }, 
                    new Currency
                    {
                        Amount = 0,
                        CurrencyName = "EUR",
                        UserBalanceId = userId
                    }
                },
            };

            await _context.UserBalances.AddAsync(balance);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
