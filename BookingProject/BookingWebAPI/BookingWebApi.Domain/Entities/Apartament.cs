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
        public string? CustomData { get; set; }
        public decimal PricePerDay { get; set; }
        public string CurrencyName { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
