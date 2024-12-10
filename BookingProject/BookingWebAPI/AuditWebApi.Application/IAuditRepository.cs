using AuditWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditRecord record);
    }
}
