using AuditWebApi.Application.DTOs;
using AuditWebApi.Domain.Constants;
using AuditWebApi.Domain.Entities;
using Contracts.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Response;
using System.Net.Http.Json;


namespace AuditWebApi.Application
{
    public interface IAuditService
    {
        Task AddUserChange(AuditChangeDto auditChangeDto);
        Task<Result<AuditUserInfoChangeDto>> GetUserByTime(string userId, DateTime timestamp);
    }

    public class AuditService : IAuditService
    {
        private readonly ILogger<AuditService> _logger;
        private readonly IAuditRepository _auditRepository;
        private readonly HttpClient _httpClient;
        private readonly string _accountUserInfoUrl;
        public AuditService(IOptions<ApiSettings> apiSettings,ILogger<AuditService> logger, IAuditRepository auditRepository, HttpClient httpClient)
        {
            _logger = logger;
            _auditRepository = auditRepository;
            _httpClient = httpClient;
            _accountUserInfoUrl = apiSettings.Value.AccountUserInfo;
        }

        public async Task AddUserChange(AuditChangeDto auditChangeDto)
        {
            try
            {
                var auditRecord = new AuditRecord()
                {
                    UserId = auditChangeDto.UserId,
                    Timestamp = auditChangeDto.Timestamp,
                    EventType = EventTypes.UserChange,
                    Changes = auditChangeDto.Changes.Select(x => new UserChange
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
                var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>($"{_accountUserInfoUrl}{userId}");
                if (userInfo is null)
                {
                    return Result<AuditUserInfoChangeDto>.Failure("User info wasn’t found");
                }

                var auditDto = new AuditUserInfoChangeDto(
                    userId,
                    user.Value.Timestamp,
                    user.Value.Changes,
                    userInfo
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
