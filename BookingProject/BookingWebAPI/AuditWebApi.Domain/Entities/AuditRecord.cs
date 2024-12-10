using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Domain.Entities
{
    public class UserChange
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
    public class AuditRecord
    {
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public List<UserChange> Changes { get; set; }   
    }
}
