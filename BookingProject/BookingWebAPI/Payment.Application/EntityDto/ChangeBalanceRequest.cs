using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.EntityDto
{
    public record ChangeBalanceRequest
    {
        public string AdminId { get; init; }
        public string UserId { get; init; }
        public string CurrenctName { get; init; }
        public decimal NewBalance { get; init; }
    }
}
