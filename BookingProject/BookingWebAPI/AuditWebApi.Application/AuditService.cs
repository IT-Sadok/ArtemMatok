using AuditWebApi.Application.DTOs;
using AuditWebApi.Domain.Entities;
using Microsoft.Extensions.Logging;
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
        Task AddUserChange(string message);
    }

    public class AuditService(
        ILogger<AuditService> _logger,
        IAuditRepository _auditRepository
    ) : IAuditService
    {
        public async Task AddUserChange(string message)
        {
            try
            {
                var auditChangeDto = JsonSerializer.Deserialize<AuditChangeDto>(message);

                if (auditChangeDto is null)
                {
                    _logger.LogError("Message is null");
                    return;
                }

                var auditRecord = new AuditRecord()
                {
                    UserId = auditChangeDto.UserId,
                    Timestamp = auditChangeDto.Timestamp,
                    EventType = "UserChange",
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
    }
}
