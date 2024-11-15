using AutoMapper;
using BookingWebApi.Application.Apartament.DTOs;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.ApartamentFeature
{
    internal class ApartamentMapper : Profile
    {
        public ApartamentMapper()
        {
            CreateMap<Apartament, ApartamentPostDto>().ReverseMap();
            CreateMap<Apartament, ApartamentGetDto>().ReverseMap();
            CreateMap<Apartament, ApartamentMigrationDto>().ReverseMap();
        }
    }
}
