using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Query;
using BookingWebApi.Helpers;

using BookingWebApi.Application.User.Services;
using Response;

namespace BookingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IAppUserService _appUserService;

        public AccountController(IAuthenticationService authService, IAppUserService appUserService)
        {
            _authService = authService;
            _appUserService = appUserService;
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

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateQuery query, CancellationToken cancellation)
        {
            var result = await _appUserService.UpdateUserAsync(UserHelpers.GetUserId(HttpContext), query, cancellation);

            return result.ToResponse();
        }

        [HttpGet("UserInfo/{userId}")]
        public async Task<IActionResult> GetUserInfoByUserId(string userId)
        {
            var result = await _appUserService.GetUserInfoAsync(userId);

            return result.ToResponse();
        }

        [HttpGet("UserRole/{userId}")]
        public async Task<IActionResult> GetUserRoleById(string userId)
        {
            var result = await _appUserService.GetUserRoleByIdAsync(userId);

            return Ok(result);
        }
    }
}
    