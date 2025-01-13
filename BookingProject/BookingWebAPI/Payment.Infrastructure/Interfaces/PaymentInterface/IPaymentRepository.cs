using Payment.Domain.Models;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Interfaces.PaymentInterface
{
    public interface IPaymentRepository
    {
        Task<bool> CreateBalanceAsync(string userId);
        Task<Result<bool>> WithdrawBalance(string userId, decimal price, string currencyName);
        Task<Result<bool>> CompensateBalance(string userId, decimal price, string currencyName);
        Task<Result<bool>> ChangeBalanceAsync(string userId, decimal amount, string currencyName);

    }
}
