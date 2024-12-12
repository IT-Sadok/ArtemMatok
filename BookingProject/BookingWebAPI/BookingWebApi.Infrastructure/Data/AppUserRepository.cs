using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Query;
using BookingWebApi.Domain.Entities;
using Contracts.DTOs;
using Microsoft.EntityFrameworkCore;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public async Task<Result<UserChangeDto>> Update(string userId, UserUpdateQuery query)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user is null)
            {
                return Result<UserChangeDto>.Failure("User wasn`t found");
            }

            var changes = new List<UserChange>();

            if(!String.IsNullOrEmpty(query.Email))
            {
                changes.Add(new UserChange
                {
                    FieldName = "Email",
                    OldValue = user.Email,
                    NewValue = query.Email
                });
                user.Email = query.Email;
            }
            if (!String.IsNullOrEmpty(query.UserName))
            {
                changes.Add(new UserChange
                {
                    FieldName = "UserName",
                    OldValue = user.UserName,
                    NewValue = query.UserName
                });
                user.UserName = query.UserName;
            }

            if (query.CustomUserData != null)
            {
                var existingData = !string.IsNullOrEmpty(user.CustomUserData)
                    ? JsonSerializer.Deserialize<List<UserCustomData>>(user.CustomUserData)
                    : new List<UserCustomData>();

                var newData = query.CustomUserData;

                foreach (var kvp in newData)
                {
                    existingData.Add(kvp);
                }

                changes.Add(new UserChange
                {
                    FieldName = "Custom Data",
                    OldValue = user.CustomUserData,
                    NewValue = JsonSerializer.Serialize(existingData)
                });

                user.CustomUserData = JsonSerializer.Serialize(existingData);
            }

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var changeEvent = new UserChangeDto(
                    user.Id,
                    DateTime.UtcNow,
                    changes
                );

                return Result<UserChangeDto>.Success(changeEvent);
            }
            catch (Exception ex)
            {
                return Result<UserChangeDto>.Failure(ex.Message);
            }
        }
    }
}
