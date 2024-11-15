using BookingWebApi.Application.DTOs.AppUserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookingWebApi.Application.UserFeature.DTOs;
using BookingWebApi.Application.UserFeature.Interfaces;
using BookingWebApi.Application.Common.Extensions;

namespace BookingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);

            return result.ToResponse();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);

            return result.ToResponse();
        }
    }
}
    