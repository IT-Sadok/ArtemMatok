using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Kafka
{
    public class UserRegisteredEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
