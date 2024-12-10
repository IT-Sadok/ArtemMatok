using AuditWebApi.Application;
using AuditWebApi.Application.DTOs;
using AuditWebApi.Infrastructure.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuditWebApi.Infrastructure.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly string _topic;
        //private readonly IAuditService _auditService;
        private readonly IServiceScopeFactory _scopeFactory;
        public KafkaConsumer(IOptions<KafkaSettings> options, ILogger<KafkaConsumer> logger, IServiceScopeFactory scopeFactory)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = options.Value.BootstrapServers,
                    GroupId = "audit",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                _topic = options.Value.Topic;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize Kafka producer", ex);
            }
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);
            _logger.LogInformation($"Subscribed to topic: {_topic}");

            Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(stoppingToken);

                        if (result != null && !string.IsNullOrEmpty(result.Message.Value))
                        {
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                                auditService.AddUserChange(result.Message.Value);
                            }

                            _logger.LogInformation($"Message processed: {result.Message.Value}");
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Kafka consume error: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unexpected error: {ex.Message}");
                    }
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }

    }
}
