using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka
{
    public class Topics
    {
        public string UserChanges { get; set; }
    }
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public Topics Topics { get; set; }
    }
    public class ConsumerSettings : KafkaSettings
    {
        public string GroupId { get; set; }
    }
}
