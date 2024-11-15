using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.ApartamentFeature.DTOs;

public record ApartamentPostDto(
    string Address,
    double Area,
    decimal Latitude,
    decimal Longtitude,
    int Bedrooms
);

