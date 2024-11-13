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
            CreateMap<AppUser, AppUserMigrationDto>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.ExternalId))
                .ReverseMap();
            CreateMap<Apartament, ApartamentMigrationDto>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.ExternalId))
                .ReverseMap();
        }
    }
}
