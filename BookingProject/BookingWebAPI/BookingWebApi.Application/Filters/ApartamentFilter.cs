using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Filters
{
    public class ApartamentFilter
    {
        public int? MinBedrooms { get; set; }
        public int? MaxArea { get; set; }
        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 10; 
    }
}
