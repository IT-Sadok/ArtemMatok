using AuditWebApi.Domain.Entities;
using Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application.DTOs
{
    public record AuditChangeDto(
         string UserId,
         DateTime Timestamp,
         List<UserChange> Changes
    );

    public record AuditUserInfoChangeDto(
        string UserId,
        DateTime Timestamp,
        List<UserChange> Changes,
        UserInfo UserInfo
    );
}
