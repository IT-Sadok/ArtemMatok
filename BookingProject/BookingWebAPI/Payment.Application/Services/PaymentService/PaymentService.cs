using Contracts.DTOs.Payment;
using DistributedLocking;
using Payment.Application.Interfaces.PaymentInterface;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Services.PaymentService
{
    public class PaymentService(
        IPaymentRepository _paymentRepository,
        IDistributedLockService _lockService
    ) : IPaymentService
    {
        public async Task<Result<bool>> CompensateBalance(BalanceRequestDto balanceDto)
        {
            var lockKey = CreateLockKey(balanceDto.UserId);
            var lockValue = string.Empty;

            try
            {
                lockValue = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10));
                if (string.IsNullOrEmpty(lockValue))
                {
                    return Result<bool>.Failure("Unable to acquire lock. Try again later.");
                }
                return await _paymentRepository.CompensateBalance(balanceDto.UserId, balanceDto.Price, balanceDto.CurrencyName);
            }
            catch(Exception ex)
            {
                return Result<bool>.Failure($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(lockValue))
                {
                    await _lockService.ReleaseLockAsync(lockKey, lockValue);
                }
            } 
        }

        public async Task<bool> CreateBalanceAsync(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                await _paymentRepository.CreateBalanceAsync(userId);
                return true;
            }

            return false;
        }

        public async Task<Result<bool>> WithdrawBalance(BalanceRequestDto balanceDto)
        {
            var lockKey = CreateLockKey(balanceDto.UserId);
            var lockValue = string.Empty;
            try
            {
                lockValue = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10));
                if (string.IsNullOrEmpty(lockValue))
                {
                    return Result<bool>.Failure("Unable to acquire lock. Try again later.");
                }

                return await _paymentRepository.WithdrawBalance(balanceDto.UserId, balanceDto.Price, balanceDto.Currency);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(lockValue))
                {
                    await _lockService.ReleaseLockAsync(lockKey, lockValue);
                }
            }
        }

        private string CreateLockKey(string userId)
        {
            return $"balance:{userId}";
        }
    }
}
