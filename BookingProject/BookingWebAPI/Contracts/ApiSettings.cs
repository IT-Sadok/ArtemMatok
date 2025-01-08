using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ApiSettings
    {
        public string MonolithUrl { get; set; }
        public string PaymentUrl { get; set; }
        public string AuditUrl { get; set; }
    }
}
