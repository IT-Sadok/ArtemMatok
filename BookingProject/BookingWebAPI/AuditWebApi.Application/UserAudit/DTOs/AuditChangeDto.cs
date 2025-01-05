using Contracts.DTOs.Audit;
using UserChangeDto = AuditWebApi.Domain.Entities.UserChange;

namespace AuditWebApi.Application.UserAudit.DTOs
{
    public record AuditChangeDto(
         string UserId,
         DateTime Timestamp,
         List<UserChangeDto> Changes
    );

    public record AuditUserInfoChangeDto(
        string UserId,
        DateTime Timestamp,
        List<UserChangeDto> Changes,
        UserInfo UserInfo
    );
}
