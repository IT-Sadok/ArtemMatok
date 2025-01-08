using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal Amount { get; set; }

        public string UserBalanceId { get; set; }

        [JsonIgnore]

        public UserBalance UserBalance { get; set; }
    }
}
