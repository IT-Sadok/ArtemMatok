﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.AppUserDTOs
{
    public record RegisterDto
    {
        public string? UserName { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
    }
}
