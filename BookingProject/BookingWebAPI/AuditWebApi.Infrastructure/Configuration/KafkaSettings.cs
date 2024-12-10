using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Infrastructure.Configuration
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
    }
}
