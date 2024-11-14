using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.ApartamentDTOs
{
    public record HostLargeApartmentDto(
        string HostId,
        Int64 ApartamentCount,
        double AverageArea
    );
}
