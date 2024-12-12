using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka
{
    public interface IBaseKafkaProducer<TKey, TValue>
    {
        Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);

    }
    public abstract class BaseKafkaProducer<TKey, TValue> : IBaseKafkaProducer<TKey, TValue>,IDisposable
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly ILogger<BaseKafkaProducer<TKey, TValue>> _logger;
        private readonly string _topic;
        protected BaseKafkaProducer(ProducerConfig config, string topic, ILogger<BaseKafkaProducer<TKey, TValue>> logger)
        {
            _topic = topic ?? throw new ArgumentNullException(nameof(topic), "Topic cannot be null or empty.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _producer = new ProducerBuilder<TKey, TValue>(config).Build();
        }

        public async Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
        {
            try
            {
                var message = new Message<TKey, TValue>
                {
                    Key = key,
                    Value = value,
                };

                var deliveryResult = await _producer.ProduceAsync(_topic, message, cancellationToken);
                _logger.LogInformation($"Message delivered to topic {_topic}, partition {deliveryResult.Partition}, offset {deliveryResult.Offset}");
            }
            catch (ProduceException<TKey, TValue> ex)
            {
                _logger.LogError($"Error producing message to topic {_topic}: {ex.Error.Reason}");
                throw;
            }
        }


        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
