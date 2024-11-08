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
        public double Latitude { get; set; }
        public double Longitude { get; set; }       
        public int Bedrooms { get; set; }
        public string HostId { get; set; }  
        public AppUser Host { get; set; }
    }
}
