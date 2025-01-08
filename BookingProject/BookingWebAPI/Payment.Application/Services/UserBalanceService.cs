using Payment.Application.EntityDto;
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
    public class UserBalanceService(IUserBalanceRepository _userRepository) : IUserBalanceService
    {
        public async Task<bool> ChangeUserBalanceAsync(ChangeBalanceRequest request)
        {
            return await _userRepository.ChangeBalanceAsync(request.AdminId, request.UserId, request.CurrenctName, request.NewBalance);
        }

        public async Task<UserBalanceResponse?> GetUSerBalanceAsync(string userId)
        {
            return await _userRepository.GetUserBalance(userId);
        }
    }
}

