using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Response;
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
                        CurrencyName = "USD"
                    },
                    new Currency
                    {
                        Amount = 0,
                        CurrencyName = "EUR"
                    }
                }
            };


            await _context.UserBalances.AddAsync(balance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Result<bool>> CompensateMoney(string userId, decimal price, string currencyName)
        {
            return await UpdateCurrencyAmount(userId, price, currencyName);
        }

        public async Task<Result<bool>> ReserveMoney(string userId, decimal price, string currencyName)
        {
            var checkBalance = await CheckBalance(userId, price, currencyName);
            if (!checkBalance.IsSuccess) return Result<bool>.Failure(checkBalance.ErrorMessage);

            return await UpdateCurrencyAmount(userId, -price, currencyName);
        }
        private async Task<Result<bool>> UpdateCurrencyAmount(string userId, decimal amount, string currencyName)
        {
            var currency = await GetCurrency(userId, currencyName);
            if (!currency.IsSuccess) return Result<bool>.Failure(currency.ErrorMessage);

            currency.Value.Amount += amount;

            _context.Entry(currency.Value).Property(c => c.Amount).IsModified = true;
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }


        private async Task<Result<bool>> CheckBalance(string userId, decimal price, string currencyName)
        {
            var currency = await GetCurrency(userId, currencyName);

            if (!currency.IsSuccess) return Result<bool>.Failure(currency.ErrorMessage);

            if (currency.Value.Amount < price)
            {
                return Result<bool>.Failure("Insufficient funds in the selected currency.");
            }

            return Result<bool>.Success(true);
        }

        private async Task<Result<Currency>> GetCurrency(string userId, string currencyName)
        {
            var paymentBalance = await _context.UserBalances
               .Include(x => x.Currencies)
               .FirstOrDefaultAsync(x => x.UserId == userId);

            if (paymentBalance is null)
            {
                return Result<Currency>.Failure("User does not have a balance account.");
            }

            var currency = paymentBalance.Currencies.FirstOrDefault(x => x.CurrencyName == currencyName);
            if (currency == null)
            {
                return Result<Currency>.Failure($"Currency '{currencyName}' not found in user's account.");
            }

            return Result<Currency>.Success(currency);
        }

    }
}


