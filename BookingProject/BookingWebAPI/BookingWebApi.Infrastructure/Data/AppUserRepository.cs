using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Query;
using BookingWebApi.Domain.Entities;
using Contracts.DTOs;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;
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
        ApplicationDbContext _context,
        IUserManagerDecorator<AppUser> _userManager
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

        public async Task<Result<UserInfoChangesDto>> Update(string userId, UserUpdateQuery query)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user is null)
            {
                return Result<UserInfoChangesDto>.Failure("User wasn`t found");
            }

            var changes = new List<UserChange>();

            if(!String.IsNullOrEmpty(query.Email))
            {
                UpdateField(nameof(query.Email), user.UserName, query.UserName, changes);
                user.Email = query.Email;
            }
            if (!String.IsNullOrEmpty(query.UserName))
            {
                UpdateField(nameof(query.UserName), user.UserName, query.UserName, changes);
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

                UpdateField(nameof(query.CustomUserData), user.CustomUserData, JsonSerializer.Serialize(existingData), changes);

                user.CustomUserData = JsonSerializer.Serialize(existingData);
            }

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();


                var userInfoChanges = new UserInfoChangesDto(
                    new UserChangeDto(
                        user.Id,
                        DateTime.UtcNow,
                        changes
                    ),
                    new UserInfo(
                        user.UserName,
                        user.Email
                    )
                );

                return Result<UserInfoChangesDto>.Success(userInfoChanges);
            }
            catch (Exception ex)
            {
                return Result<UserInfoChangesDto>.Failure(ex.Message);
            }
        }

        private void UpdateField<T>(string fieldName, T oldValue, T newValue, List<UserChange> changes)
        {
            if(!EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                changes.Add(new UserChange
                {
                    FieldName = fieldName,
                    OldValue = oldValue.ToString(),
                    NewValue = newValue.ToString()
                });
            }
        }

        public async Task<Result<UserInfo>> GetUserInfoById(string userId)
        {
            var result = await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserInfo(x.UserName, x.Email))
                .FirstOrDefaultAsync();
                
            if(result is null)
            {
                return Result<UserInfo>.Failure("User wasn`t found");
            }

            return Result<UserInfo>.Success(result);    
        }

        public async Task<string> GetRoleById(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            var roles = await _userManager.GetUserRoles(user);

            return roles.FirstOrDefault();
        }
    }
}
