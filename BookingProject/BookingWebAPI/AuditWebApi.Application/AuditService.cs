using AuditWebApi.Application.DTOs;
using AuditWebApi.Domain.Constants;
using AuditWebApi.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuditWebApi.Application
{
    public interface IAuditService
    {
        Task AddUserChange(AuditChangeDto auditChangeDto);
        Task<Result<AuditChangeDto>> GetUserByTime(string userId, DateTime timestamp);
    }

    public class AuditService(
        ILogger<AuditService> _logger,
        IAuditRepository _auditRepository,
        IMapper _mapper
    ) : IAuditService
    {
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

        public async Task<Result<AuditChangeDto>> GetUserByTime(string userId, DateTime timestamp)
        {
            var user = await _auditRepository.GetUserByTime(userId, timestamp);

            if(!user.IsSuccess)
            {
                return Result<AuditChangeDto>.Failure(user.ErrorMessage);
            }

            var auditDto = _mapper.Map<AuditChangeDto>(user.Value);

            return Result<AuditChangeDto>.Success(auditDto);
        }
    }
}
