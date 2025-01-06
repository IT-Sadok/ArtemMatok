using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Query;
using Contracts.DTOs.Audit;
using Kafka;
using Microsoft.Extensions.Logging;
using Redis;
using Response;
using System.Text.Json;



namespace BookingWebApi.Application.User.Services
{
    public interface IAppUserService
    {
        Task<Result<UserChangeDto>> UpdateUserAsync(string userId, UserUpdateQuery query, CancellationToken cancellationToken);
        Task<Result<UserInfo>> GetUserInfoAsync(string userId);  
    }
    public class AppUserService(
        IAppUserRepository _appUserRepository,
        ILogger<AppUserService> _logger,
        IUserChangeKafkaProducer _kafka,
        IRedisCacheService _cache
    ) : IAppUserService
    {
        public async Task<Result<UserInfo>> GetUserInfoAsync(string userId)
        {
            var cacheKey = $"User_{userId}";
            var cacheUserInfo = await _cache.GetAsync<UserInfo>(cacheKey);

            if (cacheUserInfo != null) return Result<UserInfo>.Success(cacheUserInfo);

            var userInfo = await _appUserRepository.GetUserInfoById(userId);
            if (userInfo is null) return Result<UserInfo>.Failure(userInfo.ErrorMessage);


            await _cache.SetAsync(cacheKey, new UserInfo(userInfo.Value.UserName, userInfo.Value.Email), TimeSpan.FromHours(1));

            return Result<UserInfo>.Success(userInfo.Value);
        }

        public async Task<Result<UserChangeDto>> UpdateUserAsync(string userId, UserUpdateQuery query, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.Update(userId, query);

            if (!result.IsSuccess)
            {
                return Result<UserChangeDto>.Failure(result.ErrorMessage);
            }

            var resSerialize = JsonSerializer.Serialize(result.Value);

            await _cache.SetAsync(userId, new { result.Value.userInfo.UserName, result.Value.userInfo.Email }, TimeSpan.FromHours(1));

            _logger.LogInformation("Sending...");
            await _kafka.ProduceAsync(userId,resSerialize, cancellationToken);


            return Result<UserChangeDto>.Success(result.Value.userChanges);
        }
    }
}
