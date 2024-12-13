using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public abstract class BaseKafkaConsumer<TKey, TValue> : BackgroundService
{
    private readonly ILogger<BaseKafkaConsumer<TKey, TValue>> _logger;
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _topic;

    protected BaseKafkaConsumer(
        ConsumerConfig config,
        string topic,
        IServiceScopeFactory scopeFactory,
        ILogger<BaseKafkaConsumer<TKey, TValue>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _topic = topic ?? throw new ArgumentNullException(nameof(topic));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

        _consumer = new ConsumerBuilder<TKey, TValue>(config)
            .SetErrorHandler((_, e) => _logger.LogError($"Kafka error: {e.Reason}"))
            .Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //await Task.Yield();
        _consumer.Subscribe(_topic);
        _logger.LogInformation($"Subscribed to topic: {_topic}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);

                    if (result != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        await ProcessMessageAsync(result.Message.Key, result.Message.Value, scope.ServiceProvider, stoppingToken);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Kafka consume error: {ex.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Kafka consumer cancellation requested.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error: {ex.Message}");
                }
            }
        }
        finally
        {
            _consumer.Close();
        }
    }

    protected abstract Task ProcessMessageAsync(TKey key, TValue value, IServiceProvider serviceProvider, CancellationToken cancellationToken);

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
