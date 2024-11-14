using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.DTOs.ApartamentDTOs
{
    public record TotalAreaCountBySourceDto(
        string SourceCompanyId,
        double TotalArea,
        Int64 ApartamentCount
    );
}
