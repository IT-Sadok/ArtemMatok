using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Audit
{
    public record UserInfo(
      string UserName,
      string Email
    );
    public record UserInfoChangesDto(
        UserChangeDto userChanges,
        UserInfo userInfo
    );
}
