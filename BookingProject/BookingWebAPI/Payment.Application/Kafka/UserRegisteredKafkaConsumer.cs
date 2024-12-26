using Confluent.Kafka;
using Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Application.Interfaces.PaymentInterface;
using Payment.Infrastructure.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Payment.Application.Kafka
{
    public class UserRegisteredKafkaConsumer : BaseKafkaConsumer<string, string>
    {
        private readonly ILogger<UserRegisteredKafkaConsumer> _logger;

        public UserRegisteredKafkaConsumer(
            IOptions<ConsumerSettings> options,
            IServiceScopeFactory scopeFactory,
            ILogger<UserRegisteredKafkaConsumer> logger)
            : base(
                new ConsumerConfig
                {
                    BootstrapServers = options.Value.BootstrapServers,
                    GroupId = options.Value.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                },
                options.Value.Topics.UserRegistered,
                scopeFactory,
                logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ProcessMessageAsync(string key, string value, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            try
            {
                var userBalanceService = serviceProvider.GetRequiredService<IPaymentService>();
                var userRegisteredEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(value);

                if (userRegisteredEvent == null)
                {
                    _logger.LogWarning("Deserialized message is null.");
                    return;
                }

                await userBalanceService.CreateBalanceAsync(userRegisteredEvent.UserId);
                _logger.LogInformation($"User balance created for userId: {userRegisteredEvent.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing UserRegistered event: {ex.Message}");
            }
        }
    }
}
