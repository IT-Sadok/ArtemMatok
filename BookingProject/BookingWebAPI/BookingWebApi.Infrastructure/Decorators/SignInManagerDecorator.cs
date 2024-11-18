using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace BookingWebApi.Infrastructure.Decorators
{
    public class SignInManagerDecorator<T> : ISignInManagerDecorator<T> where T : AppUser
    {
        private readonly SignInManager<T> _signInManager;

        public SignInManagerDecorator(SignInManager<T> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(T user, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
    }
}
