using AutoMapper;
using BookingWebApi.Application.User.DTOs;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<AppUserMigrationDto, AppUser>();
        }
    }
}
