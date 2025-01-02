using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get;set; }

        public int ApartamentId { get; set; }
        public Apartament Apartament { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
