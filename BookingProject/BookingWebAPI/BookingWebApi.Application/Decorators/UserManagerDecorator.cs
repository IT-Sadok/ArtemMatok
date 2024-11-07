using BookingWebApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Decorators
{
    public class UserManagerDecorator<T> : IUserManagerDecorator<T> where T : AppUser
    {
        private readonly UserManager<T> _userManager;

        public UserManagerDecorator(UserManager<T> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(T user, string userRole)
        {
            return await _userManager.AddToRoleAsync(user, userRole);   
        }

        public async Task<bool> CheckPasswordAsync(T user, string password)
        {
           return await _userManager.CheckPasswordAsync(user, password);    
        }

        public async Task<IdentityResult> CreateAsync(T user, string password)
        {
           return await _userManager.CreateAsync(user, password);   
        }

        public async Task<T> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetRolesAsync(T user)
        {
            return await _userManager.GetRolesAsync(user);  
        }
    }
}
