using BookingWebApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Decorators
{
    public interface IUserManagerDecorator<T> where T : AppUser
    {
        Task<T> FindByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(T user, string password);
        Task<IdentityResult> AddToRoleAsync(T user, string userRole);
        Task<IList<string>?> GetUserRoles(T user);
    }
}
