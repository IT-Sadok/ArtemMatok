    using BookingWebApi.Application.User.Interfaces;
using Confluent.Kafka;
using Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Kafka
{


    public class UserChangeKafkaProducer : BaseKafkaProducer<string, string>, IUserChangeKafkaProducer
    {
        private readonly ILogger<UserChangeKafkaProducer> _logger;

        public UserChangeKafkaProducer(
            IOptions<KafkaSettings> kafkaSettings,
            ILogger<UserChangeKafkaProducer> logger)
            : base(
                new ProducerConfig
                {
                    BootstrapServers = kafkaSettings.Value.BootstrapServers
                },
                kafkaSettings.Value.Topics.UserChanges,
                logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
