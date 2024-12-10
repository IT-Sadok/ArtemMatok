using BookingWebApi.Application.Common.Interfaces;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingWebApi.Application.User.Services
{
    public interface IAppUserService
    {
        Task<Result<UserChangeDto>> UpdateUser(string userId, UserUpdateQuery query, CancellationToken cancellationToken);
    }
    public class AppUserService(
        IAppUserRepository _appUserRepository,
        ILogger<AppUserService> _logger,
        IKafkaProducer _kafka
    ) : IAppUserService
    {
        public async Task<Result<UserChangeDto>> UpdateUser(string userId, UserUpdateQuery query, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.Update(userId, query);

            if (!result.IsSuccess)
            {
                return Result<UserChangeDto>.Failure(result.ErrorMessage);
            }
            _logger.LogInformation("Sending ");
            await _kafka.ProduceAsync<UserChangeDto>("audit-changes", result.Value, cancellationToken);


            return Result<UserChangeDto>.Success(result.Value);
        }
    }
}
