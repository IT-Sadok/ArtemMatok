using BookingWebApi.Application.Common.Interfaces;
using BookingWebApi.Application.User.Services;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Data;
using Confluent.Kafka;
using Contracts.DTOs.Payment;
using Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using static System.Formats.Asn1.AsnWriter;

namespace BookingWebApi.Infrastructure.Kafka
{
    public class PaymentReplenishmentBalanceConsumer : BaseKafkaConsumer<string, string>
    {
        private readonly ILogger<PaymentReplenishmentBalanceConsumer> _logger;
        public PaymentReplenishmentBalanceConsumer(IOptions<ConsumerSettings> options, IServiceScopeFactory scopeFactory, ILogger<PaymentReplenishmentBalanceConsumer> logger) : base(
            new ConsumerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                GroupId = options.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            },
            options.Value.Topics.ReplenishmentBalance,
            scopeFactory,
            logger)
        {
            _logger = logger;
        }
        protected override async Task ProcessMessageAsync(string key, string value, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            try
            {
                using var transaction = await unitOfWork.BeginTransactionAsync();
                var userService = scope.ServiceProvider.GetRequiredService<IAppUserService>();
                var processedEventRepository = scope.ServiceProvider.GetRequiredService<IProcessedEventRepository>();

                var alreadyProcessed = await processedEventRepository.ExistProcessedEventAsync(key);
                if (alreadyProcessed)
                {
                    _logger.LogInformation($"Event {key} already processed. Skipping.");
                    return;
                }

                var balanceDto = JsonSerializer.Deserialize<BalanceRequestDto>(value);
                if (balanceDto is null)
                {
                    _logger.LogError($"Event {key};BalanceDto is null");
                    return;
                }

                var points = balanceDto.Price * 0.1m;
                var resultComplimentaryPoints = await userService.UpdateComplimentaryPoints(balanceDto.UserId, points);
                if (!resultComplimentaryPoints.IsSuccess)
                {
                    _logger.LogError("Updating ComplimentaryPoints went wrong ");
                    return;
                }

                ProcessedEvent processedEvent = new ProcessedEvent { ProcessedEventId = key };
                var resultProcessedEvent = await processedEventRepository.AddProcessedEventAsync(processedEvent);
                if(!resultProcessedEvent.IsSuccess)
                {
                    _logger.LogError("Adding ProcessedEvent went wrong ");
                    return;
                }
                _logger.LogInformation("Success added ProcessedEvent");
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                _logger.LogError($"Error processing message: {ex.Message}");
            }
        }
    }
}
