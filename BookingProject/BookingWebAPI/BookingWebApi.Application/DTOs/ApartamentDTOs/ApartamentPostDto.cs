using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.ApartamentDTOs
{
    public record ApartamentPostDto(
        string Address,
        double Area,
        double Latitude, 
        double Longtitude,
        int Bedrooms
    );
}
