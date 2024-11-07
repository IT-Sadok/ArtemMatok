using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.AppUserDTOs
{
    public record NewUserDto
    {
        public string? Email { get; init; }
        public string? Username { get; init; }
        public string? Token { get; init; }
    }
}
