using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class UserBalance
    {
        [Key]
        public int UserBalanceId { get; set; }
        public string UserId { get; set; }
        public List<Currency>? Currencies {get;set;}
    }
}
