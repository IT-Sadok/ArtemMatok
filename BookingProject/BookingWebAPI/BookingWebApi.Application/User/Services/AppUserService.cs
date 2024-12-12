using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Query;
using Contracts.DTOs;
using Kafka;
using Microsoft.Extensions.Logging;
using Response;
using System.Text.Json;



namespace BookingWebApi.Application.User.Services
{
    public interface IAppUserService
    {
        Task<Result<UserChangeDto>> UpdateUser(string userId, UserUpdateQuery query, CancellationToken cancellationToken);
    }
    public class AppUserService(
        IAppUserRepository _appUserRepository,
        ILogger<AppUserService> _logger,
        IBaseKafkaProducer<string,string> _kafka
    ) : IAppUserService
    {
        public async Task<Result<UserChangeDto>> UpdateUser(string userId, UserUpdateQuery query, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.Update(userId, query);

            if (!result.IsSuccess)
            {
                return Result<UserChangeDto>.Failure(result.ErrorMessage);
            }

            var resSerialize = JsonSerializer.Serialize(result.Value);

            _logger.LogInformation("Sending...");
            await _kafka.ProduceAsync(userId,resSerialize, cancellationToken);


            return Result<UserChangeDto>.Success(result.Value);
        }
    }
}
