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
    public class DistributedLockService(IRedisCacheService _redisCacheService): IDistributedLockService
    {
        public async Task<string> AcquireLockAsync(string key, TimeSpan expiration)
        {
            var lockKey = $"lock:{key}";
            var lockValue = Guid.NewGuid().ToString();

            var existingLock = await _redisCacheService.GetAsync<string>(lockKey);
            if (existingLock != null) return null;

            await _redisCacheService.SetAsync(lockKey, lockValue, expiration);
            return lockValue;
        }

        public async Task<bool> ReleaseLockAsync(string key , string lockValue)
        {
            var lockKey = $"lock{key}";
            var existingValue = await _redisCacheService.GetAsync<string>(lockKey);

            if(existingValue == lockValue)
            {
                await _redisCacheService.RemoveAsync(lockKey);
                return true;
            }

            return false;
        }
    }
}
