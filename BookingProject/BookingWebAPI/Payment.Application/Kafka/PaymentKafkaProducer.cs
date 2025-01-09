using Confluent.Kafka;
using Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Kafka
{
    public interface IPaymentKafkaProducer : IBaseKafkaProducer<string, string>
    {
    }

    public class PaymentKafkaProducer : BaseKafkaProducer<string, string>, IPaymentKafkaProducer
    {
        private readonly ILogger<PaymentKafkaProducer> _logger;

        public PaymentKafkaProducer(
            IOptions<KafkaSettings> kafkaSettings,
            ILogger<PaymentKafkaProducer> logger)
            : 
            base(
                new ProducerConfig
                {
                    BootstrapServers = kafkaSettings.Value.BootstrapServers,
                    EnableIdempotence = true,
                    Acks = Acks.All,
                    MaxInFlight = 5,
                    TransactionalId = "payment-producer"
                },
                kafkaSettings.Value.Topics.ReplenishmentBalance,
                logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
