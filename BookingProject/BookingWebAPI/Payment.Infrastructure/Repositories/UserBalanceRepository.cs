using Contracts.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.Domain.Models;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces;
using Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class UserBalanceRepository(
            PaymentDbContext _context,
            ILogger<UserBalanceRepository> _logger,
            IMonolithClient _monolithClient,
            IRedisCacheService _cacheService
        ) : IUserBalanceRepository
    {
        public async Task<bool> ChangeBalanceAsync(string adminId, string userId, string currencyName, decimal newBalance)
        {
            var role = await  _monolithClient.GetRoleByIdAsync(adminId);

            if (role != "Admin")
            {
                return false;
            }

            var balance = await _context.UserBalances.Include(x => x.Currencies)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            foreach(var item in balance.Currencies)
            {
                if(item.CurrencyName == currencyName)
                {
                    item.Amount = newBalance;
                }
            }

             _context.UserBalances.Update(balance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserBalanceResponse?> GetUserBalance(string userId)
        {
            string cacheKey = $"user:{userId}";

            var cacheData = await _cacheService.GetAsync<UserBalanceResponse>(cacheKey);
            if(cacheData != null)
            {
                return cacheData;
            }

            var userInfoResult = await _monolithClient.GetUserInfoAsync(userId);
            if(!userInfoResult.IsSuccess)
            {
                _logger.LogError($"Failed to fetch user info for userId {userId}: {userInfoResult.ErrorMessage}");
                return null;
            }

            var userInfo = userInfoResult.Value;

            var balance = await _context.UserBalances.Include(x => x.Currencies)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if(balance == null)
            {
                _logger.LogError($"Balance not found for userId {userId}");
                return null;
            }

            var currencyDto = balance.Currencies
                .Select(x => new CurrencyDto
                {
                    CurrenctName = x.CurrencyName,
                    Amount = x.Amount
                }).ToList();

            var response = new UserBalanceResponse
            {
                UserId = userId,
                Email = userInfo.Email,
                UserName = userInfo.UserName,
                Currencies = currencyDto
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

            return response;
        }

        
    }
}
