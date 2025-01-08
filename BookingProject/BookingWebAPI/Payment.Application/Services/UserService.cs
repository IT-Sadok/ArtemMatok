using Payment.Application.Interfaces;
using Payment.Domain.Models;
using Payment.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Services
{
    public class UserService(IUserBalanceRepository _userRepository) : IUserService
    {
        public async Task<UserBalanceResponse?> GetUSerBalanceAsync(string userId)
        {
            return await _userRepository.GetUserBalance(userId);
        }
    }
}
