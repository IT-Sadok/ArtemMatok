using BookingWebApi.Application.Decorators;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Models;
using BookingWebApi.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserManagerDecorator<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ISignInManagerDecorator<AppUser> _signInManager;
        private readonly IValidator<RegisterDto> _validatorRegisterDto;
        private readonly IValidator<LoginDto> _validatoLoginDto;

        public AuthenticationService(IUserManagerDecorator<AppUser> userManager, ITokenService tokenService, ISignInManagerDecorator<AppUser> signInManager, IValidator<RegisterDto> validatorRegisterDto, IValidator<LoginDto> validatoLoginDto)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _validatorRegisterDto = validatorRegisterDto;
            _validatoLoginDto = validatoLoginDto;
        }

        public async Task<ApiResponse<NewUserDto>> Login(LoginDto loginDto)
        {
            var resultValidator = await _validatoLoginDto.ValidateAsync(loginDto);

            if (!resultValidator.IsValid)
            {
                var errorsValidation = resultValidator.Errors.Select(x => x.ErrorMessage).ToList();
                return new ApiResponse<NewUserDto>(errorMessages: "Validation failed", errors: errorsValidation);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return new ApiResponse<NewUserDto>(errorMessages: "Login wasn`t successful", errors: new List<string>() { "User wasn`t found" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return new ApiResponse<NewUserDto>(errorMessages: "Login wasn`t successful", errors: new List<string>() { "Email not found and/or password incorrect" });
            }

            var newUserDto = new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };

            return new ApiResponse<NewUserDto>(newUserDto, "Successful login");
        }

        public async Task<ApiResponse<NewUserDto>> Register(RegisterDto registerDto)
        {
            var resultValidator = await _validatorRegisterDto.ValidateAsync(registerDto);

            if (!resultValidator.IsValid)
            {
                var errorsValidation = resultValidator.Errors.Select(x => x.ErrorMessage).ToList();
                return new ApiResponse<NewUserDto>(errorMessages: "Validation failed", errors: errorsValidation);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new ApiResponse<NewUserDto>(
                    errorMessages: "Register failed",
                    errors: new List<string> { "Email is already in use" }
                );
            }

            var appUser = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    var newUser = new NewUserDto()
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    };

                    return new ApiResponse<NewUserDto>(newUser, "Success");
                }
                else
                {
                    var errorsRole = roleResult.Errors.Select(x => x.Description).ToList();
                    return new ApiResponse<NewUserDto>(errorMessages: "Register failed", errors: errorsRole);
                }
            }
            else
            {
                var errorsUser = createdUser.Errors.Select(x => x.Description).ToList();
                return new ApiResponse<NewUserDto>(errorMessages: "Register failed", errors: errorsUser);
            }

        }
    }
}
