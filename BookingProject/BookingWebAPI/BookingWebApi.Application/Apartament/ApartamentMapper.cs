using AutoMapper;
using BookingWebApi.Application.Apartament.DTOs;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApartamentEntity = BookingWebApi.Domain.Entities.Apartament;

namespace BookingWebApi.Application.Apartament
{
    internal class ApartamentMapper : Profile
    {
        public ApartamentMapper()
        {
            CreateMap<ApartamentEntity, ApartamentPostDto>().ReverseMap();
            CreateMap<ApartamentEntity, ApartamentGetDto>().ReverseMap();
            CreateMap<ApartamentEntity, ApartamentMigrationDto>().ReverseMap();
        }
    }
}
