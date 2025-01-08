using AuditWebApi.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application.BookingAudit
{
    public interface IAuditBookingRepository
    {
        Task<Result<bool>> AddAuditBookingAsync(AuditBooking auditBooking);
        Task<Result<bool>> RemoveAuditBookingAsync(string userId, int apartamentId, DateTime startDate);
    }
}
