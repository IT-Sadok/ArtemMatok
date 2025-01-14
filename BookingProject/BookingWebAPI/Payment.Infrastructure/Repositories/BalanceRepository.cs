using Contracts.Clients;
using Contracts.DTOs.Payment;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;
using Payment.Infrastructure.DataContext;
using Redis;
using Response;


namespace Payment.Infrastructure.Repositories
{
    public interface IBalanceRepository
    {
        Task<Result<UserBalanceInfo>> GetBalanceInfoAsync(string userId);
        
    }

    public class BalanceRepository(
        PaymentDbContext _context,
        IMonolithClient _monolith,
        IRedisCacheService _cache
    ) : IBalanceRepository
    {
        public async Task<Result<UserBalanceInfo>> GetBalanceInfoAsync(string userId)
        {
            var cacheKey = $"User_{userId}";
            var cacheBalanceInfo = await _cache.GetAsync<UserBalanceInfo>(cacheKey);

            if(cacheBalanceInfo != null)
            {
                return Result<UserBalanceInfo>.Success(cacheBalanceInfo);
            }

            var userInfo = await _monolith.GetUserInfoAsync(userId);

            if (userInfo == null)
            {
                return Result<UserBalanceInfo>.Failure("User is null");
            }

            var user = userInfo.Value;

            var balance = await _context.UserBalances.Include(x => x.Currencies)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if(balance == null)
            {
                return Result<UserBalanceInfo>.Failure("User balance is null");
            }

            var currency = balance.Currencies
                .Select(x => new CurrencyDto
                {
                    Amount = x.Amount,
                    CurrecnyName = x.CurrencyName
                }).ToList();

            var userBalanceInfo = new UserBalanceInfo
            {
                UserEmail = user.Email,
                UserName = user.UserName,
                Currencies = currency
            };

            return Result<UserBalanceInfo>.Success(userBalanceInfo);

        }
    }
}
