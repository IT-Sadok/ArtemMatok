using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User.Interfaces
{
    public interface IAppUserRepository
    {
        Task<bool> UserExists(string userId);
        Task<bool> UserExistsByIdAndCompany(string externalId, string sourceCompanyId);
    }
}
