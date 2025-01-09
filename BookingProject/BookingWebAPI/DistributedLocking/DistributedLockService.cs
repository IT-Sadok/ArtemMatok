using Polly.Retry;
using Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLocking
{
    public interface IDistributedLockService
    {
        Task<string> AcquireLockAsync(string key, TimeSpan expiration);
        Task<bool> ReleaseLockAsync(string key, string lockValue);
    }
    public class DistributedLockService(
        IRedisCacheService _redisCacheService,
        AsyncRetryPolicy _retryPolicy
    ): IDistributedLockService
    {
        public async Task<string> AcquireLockAsync(string key, TimeSpan expiration)
        {
            var lockKey = CreateLockKey(key);
            var lockValue = Guid.NewGuid().ToString();

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var existingLock = await _redisCacheService.GetAsync<string>(lockKey);
                if (existingLock != null) throw new InvalidOperationException("Lock is already created");

                await _redisCacheService.SetAsync(lockKey, lockValue, expiration);
                return lockValue;   
            });
        }

        public async Task<bool> ReleaseLockAsync(string key , string lockValue)
        {
            var lockKey = CreateLockKey(key);
            var existingValue = await _redisCacheService.GetAsync<string>(lockKey);

            if(existingValue == lockValue)
            {
                await _redisCacheService.RemoveAsync(lockKey);
                return true;
            }

            return false;
        }

        private string CreateLockKey(string key)
        {
            return $"lock:{key}";
        }
    }
}
