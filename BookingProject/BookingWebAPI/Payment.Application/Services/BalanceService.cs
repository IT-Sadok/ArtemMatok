using Payment.Application.Dtos;
using Payment.Domain.Models;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Payment.Infrastructure.Repositories;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Services
{

    public interface IBalanceService
    {
        Task<Result<UserBalanceInfo>> GetBalanceInfoAsync(string userId);
        Task<Result<bool>> ChangeBalanceAsync(ChangeBalanceRequestDto request);
    }
    public class BalanceService(IBalanceRepository _balanceRepository, IPaymentRepository _paymentRepository) : IBalanceService
    {
        public async Task<Result<bool>> ChangeBalanceAsync(ChangeBalanceRequestDto request)
        {
            var result = await _paymentRepository.ChangeBalanceAsync(request.UserId, request.Amount, request.CurrencyName);

            if(!result.IsSuccess)
            {
                return Result<bool>.Failure("Something went wrong");
            }

            return result;
        }

        public async Task<Result<UserBalanceInfo>> GetBalanceInfoAsync(string userId)
        {
            if(userId is null)
            {
                return Result<UserBalanceInfo>.Failure("UserId is null");
            }

            var userBalanceInfo = await _balanceRepository.GetBalanceInfoAsync(userId); 

            if(userBalanceInfo == null)
            {
                return Result<UserBalanceInfo>.Failure("Not balance info for this user.");
            }

            return userBalanceInfo;
        }
    }
}
