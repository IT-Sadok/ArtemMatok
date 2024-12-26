using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class UserBalance
    {
        public int UserBalanceId { get; set; }
        public string UserId { get; set; }
        public decimal UAH { get; set; }
        public decimal USD { get; set; }
        public decimal EUR { get; set; }
    }
}
