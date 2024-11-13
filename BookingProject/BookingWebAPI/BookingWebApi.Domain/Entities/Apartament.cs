using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Domain.Entities
{
    public class Apartament
    {
        public int ApartamentId { get; set; }  
        public string Address { get; set; }
        public double Area { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }       
        public int Bedrooms { get; set; }
        public string? ExternalId { get; set; }
        public string? SourceCompanyId { get; set; }
        public string HostId { get; set; }  
        public AppUser Host { get; set; }
    }
}
