using BookingWebApi.Application.Decorators;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Services;
using BookingWebApi.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Tests.Services
{
    public class AuthenticationServiceTest
    {
        private readonly Mock<IUserManagerDecorator<AppUser>> _userManager;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<ISignInManagerDecorator<AppUser>> _signInManager;
        private readonly Mock<IValidator<RegisterDto>> _validatorRegisterDto;
        private readonly Mock<IValidator<LoginDto>> _validatorLoginDto;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTest()
        {
            _userManager = new Mock<IUserManagerDecorator<AppUser>>();
            _tokenService = new Mock<ITokenService>();
            _signInManager = new Mock<ISignInManagerDecorator<AppUser>>();
            _validatorRegisterDto = new Mock<IValidator<RegisterDto>>();    
            _validatorLoginDto = new Mock<IValidator<LoginDto>>();
            _authenticationService = new AuthenticationService(
                _userManager.Object, 
                _tokenService.Object, 
                _signInManager.Object, 
                _validatorRegisterDto.Object,
                _validatorLoginDto.Object
            );
        }

        [Fact]
        public async Task AuthenticationService_Register_ReturnSuccess()
        {
            //Arange
            var registerDto = new RegisterDto { Email = "test@gmail.com", Password = "Testtest1@", UserName = "Test" };


            _validatorRegisterDto.Setup(x => x.ValidateAsync(registerDto,default))
                .ReturnsAsync(new ValidationResult());
            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);


            //Act
            var result = await _authenticationService.Register(registerDto);

            //Assert
            result.Should().NotBeNull();    
           result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task AuthenticationService_Login_ReturnSuccess()
        {
            //Arrange
            var loginDto = new LoginDto { Email="test@gmail.com", Password="Testtest1@" };
            var appUser = new AppUser { Email = loginDto.Email };

            _validatorLoginDto.Setup(x=>x.ValidateAsync(loginDto,default)) 
                .ReturnsAsync(new ValidationResult());
            _userManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(appUser);
            _signInManager.Setup(x => x.CheckPasswordSignInAsync(appUser, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            //Act
            var result = await _authenticationService.Login(loginDto);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task AuthenticationService_Register_WithInvalidData_ReturnsFailure()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "invalidEmail", Password = "short", UserName = "" };
            var validationFailure = new ValidationFailure("Email", "Invalid email format");

            _validatorRegisterDto.Setup(x => x.ValidateAsync(registerDto, default))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { validationFailure }));

            // Act
            var result = await _authenticationService.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Invalid email format");
        }

        [Fact]
        public async Task AuthenticationService_Register_WhenUserCreationFails_ReturnsFailure()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@gmail.com", Password = "Testtest1@", UserName = "Test" };

            _validatorRegisterDto.Setup(x => x.ValidateAsync(registerDto, default))
                .ReturnsAsync(new ValidationResult());
            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

            // Act
            var result = await _authenticationService.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("User creation failed");
        }

        [Fact]
        public async Task AuthenticationService_Login_WithInvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "nonexistent@gmail.com", Password = "WrongPassword" };

            _validatorLoginDto.Setup(x => x.ValidateAsync(loginDto, default))
                .ReturnsAsync(new ValidationResult());
            _userManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync((AppUser)null);  

            // Act
            var result = await _authenticationService.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Login wasn`t successful");
        }

        [Fact]
        public async Task AuthenticationService_Login_WithIncorrectPassword_ReturnsFailure()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@gmail.com", Password = "WrongPassword" };
            var appUser = new AppUser { Email = loginDto.Email };

            _validatorLoginDto.Setup(x => x.ValidateAsync(loginDto, default))
                .ReturnsAsync(new ValidationResult());
            _userManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(appUser);
            _signInManager.Setup(x => x.CheckPasswordSignInAsync(appUser, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Failed); 

            // Act
            var result = await _authenticationService.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }

    }


}
