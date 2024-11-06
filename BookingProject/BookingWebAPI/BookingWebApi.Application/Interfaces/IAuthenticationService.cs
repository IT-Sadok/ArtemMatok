﻿using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Application.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<NewUserDto>> Register(RegisterDto registerDto);    
        Task<Result<NewUserDto>> Login(LoginDto loginDto); 
    }
}
