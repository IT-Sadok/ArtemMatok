using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Audit
{
    public record AuditBookingCreateDto(
        string UserId,
        int ApartamentId,
        DateTime StartDate,
        decimal TotalAmount,
        string CurrencyName
    );
}
