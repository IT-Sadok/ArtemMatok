using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Apartament.DTOs;

public record ApartamentGetDto()
{
    public int ApartamentId { get; init; }
    public int Area { get; init; }
    public int Bedrooms { get; init; }
    public string HostId { get; init; }
}

