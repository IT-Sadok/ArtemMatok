using AutoMapper;
using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingWebApi.Application.DTOs.AppUserDTOs;

namespace BookingWebApi.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Apartament, ApartamentPostDto>().ReverseMap();
            CreateMap<Apartament, ApartamentGetDto>().ReverseMap();
            CreateMap<AppUserMigrationDto, AppUser>();
            CreateMap<Apartament, ApartamentMigrationDto>().ReverseMap();
        }
    }
}
