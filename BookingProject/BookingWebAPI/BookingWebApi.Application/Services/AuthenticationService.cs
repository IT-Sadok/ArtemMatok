using BookingWebApi.Application.Decorators;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Models;
using BookingWebApi.Domain.Entities;
using FluentValidation;

namespace BookingWebApi.Application.Services
{
    public class AuthenticationService(
        IUserManagerDecorator<AppUser> _userManager,
        ITokenService _tokenService,
        ISignInManagerDecorator<AppUser> _signInManager,
        IValidator<RegisterDto> _validatorRegisterDto,
        IValidator<LoginDto> _validatoLoginDto
    ) : IAuthenticationService
    {
        public async Task<Result<NewUserDto>> Login(LoginDto loginDto)
        {
            var resultValidator = await _validatoLoginDto.ValidateAsync(loginDto);

            if (!resultValidator.IsValid)
            {
                var errorsValidation = resultValidator.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<NewUserDto>.Failure(errorsValidation);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Result<NewUserDto>.Failure("User wasn`t found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Result<NewUserDto>.Failure("Email not found and/or password incorrect");
            }

            var newUserDto = new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };

            return Result<NewUserDto>.Success(newUserDto);
        }

        public async Task<Result<NewUserDto>> Register(RegisterDto registerDto)
        {
            var resultValidator = await _validatorRegisterDto.ValidateAsync(registerDto);

            if (!resultValidator.IsValid)
            {
                var errorsValidation = resultValidator.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<NewUserDto>.Failure(errorsValidation);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {

                return Result<NewUserDto>.Failure("Email is already in use");
            }

            var appUser = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);


            if(!createdUser.Succeeded)
            {
                var errorsUser = createdUser.Errors.Select(x => x.Description).ToList();
                return Result<NewUserDto>.Failure(errorsUser);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                var errorsRole = roleResult.Errors.Select(x => x.Description).ToList();
                return Result<NewUserDto>.Failure(errorsRole);
            }

            var newUser = new NewUserDto()
            {
                Username = appUser.UserName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser)
            };

            return Result<NewUserDto>.Success(newUser);
        }
    }
}
