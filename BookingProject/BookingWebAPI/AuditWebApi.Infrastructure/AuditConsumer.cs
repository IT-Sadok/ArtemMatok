using AuditWebApi.Application.UserAudit;
using AuditWebApi.Application.UserAudit.DTOs;
using Confluent.Kafka;
using DnsClient.Internal;
using Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuditWebApi.Infrastructure
{
    public class AuditConsumer : BaseKafkaConsumer<string, string>
    {
        private readonly ILogger<AuditConsumer> _logger;
        public AuditConsumer(IOptions<ConsumerSettings> options, IServiceScopeFactory scopeFactory, ILogger<AuditConsumer> logger) : base(
            new ConsumerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                GroupId = options.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            },
            options.Value.Topics.UserChanges,
            scopeFactory,
            logger)
        {
            _logger = logger;
        }

        protected override async Task ProcessMessageAsync(string key, string value, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            try
            {
                var auditService = serviceProvider.GetRequiredService<IAuditService>();
                var auditChangeDto = JsonSerializer.Deserialize<AuditChangeDto>(value);
                if (auditChangeDto == null)
                {
                    _logger.LogWarning("Deserialized message is null.");
                    return;
                }

                await auditService.AddUserChange(auditChangeDto);
                _logger.LogInformation($"Message processed: {value}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
            }
        }
    }
}
