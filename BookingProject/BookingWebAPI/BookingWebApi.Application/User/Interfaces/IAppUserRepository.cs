using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Application.User.Query;
using BookingWebApi.Domain.Entities;
using Contracts.DTOs;
using Response;
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
        Task<Result<UserChangeDto>> Update(string userId, UserUpdateQuery query);
    }
}
