using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Domain.Entities;
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
        public async Task<bool> UserExistsByIdAndCompany(string externalId, string sourceCompanyId)
        {
            var user =  await _context.Users
                .Where(x => x.ExternalId == externalId && x.SourceCompanyId == sourceCompanyId)
                .FirstOrDefaultAsync();
           

            if (user == null) return false;
            return true;
        }

        public async Task<bool> UserExists(string userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return false;
            return true;
        }
    }
}
