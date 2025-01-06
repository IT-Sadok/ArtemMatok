using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingEntity = BookingWebApi.Domain.Entities.Booking;

namespace BookingWebApi.Application.Booking
{
    public class BookingMapper : Profile
    {
        public BookingMapper()
        {
            CreateMap<BookingDto, BookingEntity>();
        }
    }
}
