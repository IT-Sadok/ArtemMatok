using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.UserFeature.DTOs
{
    public record RegisterDto(
        string? UserName,
        string? Email,
        string? Password,
        string Role
    );
}
