using Payment.Application.DTOs;
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
        Task<bool> CreateBalanceAsync(string userId);
        Task<Result<bool>> ReserveBalance(BalanceRequestDto balanceDto);
        Task<Result<bool>> CompensateBalance(BalanceRequestDto balanceDto);

    }
}
