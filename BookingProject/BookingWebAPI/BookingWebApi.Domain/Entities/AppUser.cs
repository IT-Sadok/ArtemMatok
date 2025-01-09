using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? ExternalId { get; set; }
        public string? SourceCompanyId { get; set; }
        public List<Apartament> Apartaments { get; set; } = new List<Apartament>();
        public string? CustomUserData { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public decimal ComplimentaryPoints { get; set; }
    }
}
