using BookingWebApi.Application.DTOs.AppUserDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Decorators
{
    public interface ISignInManagerDecorator<T> where T : class
    {
        //var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        Task<SignInResult> CheckPasswordSignInAsync(T user, string password, bool lockoutOnFailure);   
    }
}
