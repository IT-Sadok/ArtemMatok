using Contracts.DTOs.Payment;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interfaces.PaymentInterface
{
    public interface IPaymentService
    {
        Task<Result<bool>> CreateBalanceAsync(string userId);
        Task<Result<bool>> WithdrawBalance(BalanceRequestDto balanceDto);
        Task<Result<bool>> CompensateBalance(BalanceRequestDto balanceDto);

    }
}
