using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Apartament> Apartaments { get; set; }
        public DbSet<Booking> Bookings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Apartament>()
                .HasIndex(x => new { x.ExternalId, x.SourceCompanyId })
                .IsUnique();
            builder.Entity<AppUser>()
                .HasIndex(x => new { x.ExternalId, x.SourceCompanyId })
                .IsUnique();

            builder.Entity<Booking>()
                .HasOne(x => x.User)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Booking>()
                .HasOne(x => x.Apartament)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.ApartamentId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
