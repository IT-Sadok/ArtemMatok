using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.ApartamentDTOs
{
    public record ApartamentGetDto()
    {
        public int ApartamentId { get; init; }
        public string Address { get; init; }
        public int Area { get; init; }
        public int Bedrooms { get; init; }
        public double Latitude { get; init; }
        public double Longtitude { get; init; }
        public string HostName { get; init; }
    }
}
