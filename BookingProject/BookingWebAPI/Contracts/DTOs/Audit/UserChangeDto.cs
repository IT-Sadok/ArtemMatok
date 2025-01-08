using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Audit
{
    public class UserChange
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public record UserChangeDto(
         string UserId,
         DateTime Timestamp,
         List<UserChange> Changes
    );
}
