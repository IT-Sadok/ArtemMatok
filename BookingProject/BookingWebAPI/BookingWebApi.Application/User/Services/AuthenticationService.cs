using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Response;

namespace BookingWebApi.Application.User.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUserManagerDecorator<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ISignInManagerDecorator<AppUser> _signInManager;
        private readonly IUserRegisteredKafkaProducer _kafkaProducer;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUserManagerDecorator<AppUser> userManager,
            ITokenService tokenService,
            ISignInManagerDecorator<AppUser> signInManager,
            IUserRegisteredKafkaProducer kafkaProducer,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<NewUserDto>> Login(LoginDto loginDto)
        {

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

            var roles = await _userManager.GetUserRoles(user);
            var userRole = roles.FirstOrDefault();

            if (string.IsNullOrEmpty(userRole))
            {
                return Result<NewUserDto>.Failure("Role assignment failed. No role found for the user.");
            }

            var newUserDto = new NewUserDto(
                user.UserName,
                user.Email,
                await _tokenService.CreateToken(user),
                userRole
            );

            return Result<NewUserDto>.Success(newUserDto);
        }

        public async Task<Result<NewUserDto>> Register(RegisterDto registerDto)
        {
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

            if (!createdUser.Succeeded)
            {
                var errorsUser = createdUser.Errors.Select(x => x.Description).ToList();
                return Result<NewUserDto>.Failure(errorsUser);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, registerDto.Role);
            if (!roleResult.Succeeded)
            {
                var errorsRole = roleResult.Errors.Select(x => x.Description).ToList();
                return Result<NewUserDto>.Failure(errorsRole);
            }

            var roles = await _userManager.GetUserRoles(appUser);
            var userRole = roles.FirstOrDefault();

            if (string.IsNullOrEmpty(userRole))
            {
                return Result<NewUserDto>.Failure("Role assignment failed. No role found for the user.");
            }

            try
            {
                await _kafkaProducer.ProduceUserRegisteredEventAsync(appUser.Id, appUser.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error publishing UserRegistered event: {ex.Message}");
            }

            var newUser = new NewUserDto(
                appUser.UserName,
                appUser.Email,
                await _tokenService.CreateToken(appUser),
                userRole
            );

            return Result<NewUserDto>.Success(newUser);
        }
    }
}
