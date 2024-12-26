using BookingWebApi.Application.User.Interfaces;
using Confluent.Kafka;
using Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Kafka
{
    public class UserRegisteredKafkaProducer: BaseKafkaProducer<string, string>, IUserRegisteredKafkaProducer
    {
        private readonly ILogger<UserRegisteredKafkaProducer> _logger;

        public UserRegisteredKafkaProducer(
            IOptions<KafkaSettings> kafkaSettings,
            ILogger<UserRegisteredKafkaProducer> logger)
            : base(
                new ProducerConfig
                {
                    BootstrapServers = kafkaSettings.Value.BootstrapServers
                },
                kafkaSettings.Value.Topics.UserRegistered,
                logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProduceUserRegisteredEventAsync(string userId, string email)
        {
            var eventMessage = new
            {
                UserId = userId,
                Email = email,
                RegisteredAt = DateTime.UtcNow
            };

            var serializedMessage = JsonSerializer.Serialize(eventMessage);

            await ProduceAsync(userId, serializedMessage);
            _logger.LogInformation($"UserRegistered event produced: {serializedMessage}");
        }
    }
}
