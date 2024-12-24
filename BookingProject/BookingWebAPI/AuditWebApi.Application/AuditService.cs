using AuditWebApi.Application.DTOs;
using AuditWebApi.Domain.Constants;
using AuditWebApi.Domain.Entities;
using Contracts.Clients;
using Contracts.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Response;
using System.Net.Http.Json;
using UserChangeDto = AuditWebApi.Domain.Entities.UserChange;

namespace AuditWebApi.Application
{
    public interface IAuditService
    {
        Task AddUserChange(AuditChangeDto auditChangeDto);
        Task<Result<AuditUserInfoChangeDto>> GetUserByTime(string userId, DateTime timestamp);
    }

    public class AuditService(ILogger<AuditService> logger, IAuditRepository auditRepository, IMonolithClient monolithClient) : IAuditService
    {
        private readonly ILogger<AuditService> _logger = logger;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private readonly IMonolithClient _monolithClient = monolithClient;

        public async Task AddUserChange(AuditChangeDto auditChangeDto)
        {
            try
            {
                var auditRecord = new AuditRecord()
                {
                    UserId = auditChangeDto.UserId,
                    Timestamp = auditChangeDto.Timestamp,
                    EventType = EventTypes.UserChange,
                    Changes = auditChangeDto.Changes.Select(x => new UserChangeDto
                    {
                        FieldName = x.FieldName,
                        OldValue = x.OldValue,
                        NewValue = x.NewValue,
                    }).ToList()
                };

                await _auditRepository.AddAsync(auditRecord);   
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing event: {ex.Message}");
            }
        }

        public async Task<Result<AuditUserInfoChangeDto>> GetUserByTime(string userId, DateTime timestamp)
        {
            var user = await _auditRepository.GetUserByTime(userId, timestamp);

            if(!user.IsSuccess)
            {
                return Result<AuditUserInfoChangeDto>.Failure(user.ErrorMessage);
            }

            try
            {
                var userInfo = await _monolithClient.GetUserInfoAsync(userId);
                if (!userInfo.IsSuccess)
                {
                    return Result<AuditUserInfoChangeDto>.Failure(userInfo.ErrorMessage);
                }

                var auditDto = new AuditUserInfoChangeDto(
                    userId,
                    user.Value.Timestamp,
                    user.Value.Changes,
                    userInfo.Value
                );

                return Result<AuditUserInfoChangeDto>.Success(auditDto);
            }
            catch (HttpRequestException ex)
            {

                Console.WriteLine($"Request failed: {ex.Message}");
                return Result<AuditUserInfoChangeDto>.Failure("Failed to fetch user info.");
            }
        }
    }
}
