using Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User.Interfaces
{
    public interface IUserChangeKafkaProducer : IBaseKafkaProducer<string, string>
    {
    }
}
