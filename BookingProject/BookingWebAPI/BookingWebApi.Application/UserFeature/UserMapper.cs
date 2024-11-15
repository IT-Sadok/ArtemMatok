using AutoMapper;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.UserFeature
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<AppUserMigrationDto, AppUser>();
        }
    }
}
