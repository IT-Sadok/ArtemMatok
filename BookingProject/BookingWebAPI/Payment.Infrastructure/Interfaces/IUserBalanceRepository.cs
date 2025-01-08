using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Interfaces
{
    public interface IUserBalanceRepository
    {
        Task<UserBalanceResponse?> GetUserBalance(string userId);
        Task<bool> ChangeBalanceAsync(string adminId, string userId, string currencyName, decimal newBalance);
    }
}
