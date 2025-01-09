using Contracts.DTOs.Payment;
using Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Payment.Application.Kafka;
using Payment.Domain.Models;
using Payment.Infrastructure.Interfaces.OutboxInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Payment.Application.Services
{
    public class OutboxPublisherService(
       IServiceScopeFactory _scopeFactory,
       IPaymentKafkaProducer _paymentKafkaProducer,
       ILogger<OutboxPublisherService> _logger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var events = await outboxRepository.GetUnprocessedOutboxEventsAsync();
                    foreach (var item in events)
                    {
                        var payload = JsonSerializer.Deserialize<BalanceRequestDto>(item.Payload);
                        if (payload is null) continue;

                        await _paymentKafkaProducer.ProduceAsync(item.OutboxEventId.ToString(), item.Payload, stoppingToken);
                        _logger.LogInformation("Sent kafka event");

                        var res = await outboxRepository.MarkOutboxEventAsProcessedAsync(item.OutboxEventId);
                        if (!res.IsSuccess) return;
                        _logger.LogInformation("Marked out boxEvent as processed");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing Outbox events: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
