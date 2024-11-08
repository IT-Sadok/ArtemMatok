using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.AppUserDTOs
{
    public record NewUserDto(string? Email,
        string? Username,
        string? Token,
        string? Role
    );
}
