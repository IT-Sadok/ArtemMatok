using AutoMapper;
using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Apartament, ApartamentPostDto>().ReverseMap();
            CreateMap<Apartament, ApartamentGetDto>()
                .ForMember(x => x.HostName, y => y.MapFrom(a => a.Host.UserName))
                .ReverseMap();
        }
    }
}
