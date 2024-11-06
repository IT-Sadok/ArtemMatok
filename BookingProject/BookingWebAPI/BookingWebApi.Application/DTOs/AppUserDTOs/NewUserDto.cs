using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.AppUserDTOs
{
    public class NewUserDto
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}
