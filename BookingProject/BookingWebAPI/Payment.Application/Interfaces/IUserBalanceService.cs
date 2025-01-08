using Payment.Application.EntityDto;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interfaces
{
    public interface IUserBalanceService
    {
        Task<UserBalanceResponse?> GetUSerBalanceAsync(string userId);
        Task<bool> ChangeUserBalanceAsync(ChangeBalanceRequest request);
    }
}
