using AuditWebApi.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application.UserAudit
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditRecord record);
        Task<Result<AuditRecord>> GetUserByTime(string userId, DateTime timestamp);
    }
}
