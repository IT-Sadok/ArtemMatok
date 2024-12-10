using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Common.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken);
    }
}
