using BookingWebApi.Application.Common.Interfaces;
using BookingWebApi.Infrastructure.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BookingWebApi.Infrastructure.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;
        public KafkaProducer(IOptions<KafkaSettings> options, ILogger<KafkaProducer> logger)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = options.Value.BootstrapServers,
                    AllowAutoCreateTopics = true,
                    Acks = Acks.All,
                };

                _producer = new ProducerBuilder<Null, string>(config).Build();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize Kafka producer", ex);
            }

            _logger = logger;
        }
        public async Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken)
        {
            var messageSerialize = JsonSerializer.Serialize(message);

            if(string.IsNullOrEmpty(messageSerialize))
            {
                _logger.LogError("Message is null");
                throw new InvalidOperationException("Problem with serialization");
            }

            try
            {
                var delivaryResult = await _producer.ProduceAsync(
                    topic,
                    new Message<Null, string>
                    {
                        Value = messageSerialize,
                    },
                    cancellationToken
                );

                _logger.LogInformation($"Delivared message to {delivaryResult.Value}, Offset:{delivaryResult.Offset}");

            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed:{e.Error.Reason}");
            }

            _producer.Flush(cancellationToken);
        }
    }
}
