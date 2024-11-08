using BookingWebApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Data
{
    public class AppUserRepository(
        ApplicationDbContext _context
    ) : IAppUserRepository
    {
        public async Task<bool> IsUserExist(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return false;
            return true;
        }
    }
}
