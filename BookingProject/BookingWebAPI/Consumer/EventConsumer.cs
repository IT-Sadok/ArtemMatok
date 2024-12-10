using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Consumer
{
    public class EventConsumer : BackgroundService
    {
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("audit-changes");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

                    if (consumeResult is null)
                    {
                        continue;
                    }

                    var personDes = JsonSerializer.Deserialize<object>(consumeResult.Message.Value);

                    _logger.LogInformation($"Consumed message:{personDes} at: {consumeResult.TopicPartitionOffset}");
                }
                catch (OperationCanceledException)
                {

                    throw;
                }
            }

            return Task.CompletedTask;
        }
    }
}
