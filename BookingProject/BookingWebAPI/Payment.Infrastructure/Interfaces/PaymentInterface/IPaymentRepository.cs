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
    }
}
