using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Models;
using BookingWebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Tests.Controllers
{
    public class AccountControllerTest
    {
        private readonly Mock<IAuthenticationService> _authService;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            _authService = new Mock<IAuthenticationService>();
            _controller = new AccountController(_authService.Object);
        }

        [Fact]
        public async Task AccountController_Register_ReturnOk()
        {
            //Arrange
            var registerDto = new RegisterDto { UserName = "Test", Email = "test@gmail.com", Password = "Testtest1@" };
            var newUser = new NewUserDto { Username = "testuser", Email = "testuser@example.com", Token = "testtoken" };

            _authService.Setup(service => service.Register(registerDto))
                .ReturnsAsync(new ApiResponse<NewUserDto>(newUser, "Success"));

            //Act
            var result = await _controller.Register(registerDto);


            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseValue = Assert.IsType<NewUserDto>(okResult.Value);
            Assert.Equal("testuser", responseValue.Username);
            Assert.Equal("testtoken", responseValue.Token);
        }

        [Fact]
        public async Task AccountController_Register_ReturnBadRequest()
        {
            //Arrange
            var registerDto = new RegisterDto { UserName = "Test", Email = "test@gmail.com", Password = "Testtest1@" };
            var errors = new List<string>() { "User creation failed" };

            _authService.Setup(service => service.Register(registerDto))
                .ReturnsAsync(new ApiResponse<NewUserDto>("Register failed", errors));

            //Act
            var result = await _controller.Register(registerDto);


            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<NewUserDto>>(badResult.Value);
            Assert.Equal("Register failed", apiResponse.ErrorMessage);
            Assert.Contains("User creation failed", apiResponse.Errors);
        }

        [Fact]
        public async Task AccountController_Login_ReturnOk()
        {
            //Arrange
            var loginDto = new LoginDto { Email = "test@gmail.com", Password = "Testtest1@" };
            var newUser = new NewUserDto { Email = "test@gmail.com", Username = "test", Token = "test" };

            _authService.Setup(service => service.Login(loginDto))
                .ReturnsAsync(new ApiResponse<NewUserDto>(newUser, "Successful login"));


            //Act
            var result = await _controller.Login(loginDto);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okReslt = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<NewUserDto>(okReslt.Value);
            Assert.Equal(newUser.Email, apiResponse.Email);
        }

        [Fact]
        public async Task AccountController_Login_ReturnUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "testuser@example.com", Password = "WrongPassword!" };
            var errorMessages = new List<string> { "Email not found and/or password incorrect" };

            _authService
                .Setup(service => service.Login(loginDto))
                .ReturnsAsync(new ApiResponse<NewUserDto>("Login wasn`t successful", errorMessages));

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
