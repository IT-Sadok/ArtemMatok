using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Domain.Entities
{
    public class ProcessedEvent
    {
        public string ProcessedEventId { get; set; }
        public DateTime ProccesedAt { get; set; } = DateTime.UtcNow;
    }
}
