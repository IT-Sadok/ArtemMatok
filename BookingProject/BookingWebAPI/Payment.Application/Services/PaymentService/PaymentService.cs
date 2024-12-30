using Payment.Application.Interfaces.PaymentInterface;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Services.PaymentService
{
    public class PaymentService(IPaymentRepository _paymentRepository) : IPaymentService
    {
        public async Task<bool> CreateBalanceAsync(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                await _paymentRepository.CreateBalanceAsync(userId);
                return true;
            }

            return false;
        }
    }
}
