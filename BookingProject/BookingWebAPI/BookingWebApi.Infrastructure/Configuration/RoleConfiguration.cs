using BookingWebApi.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = UserRoles.Admin,
                    NormalizedName = UserRoles.Admin.ToUpper()
                },
                new IdentityRole
                {
                    Name = UserRoles.User,
                    NormalizedName = UserRoles.User.ToUpper()
                },
                new IdentityRole
                {
                    Name = UserRoles.Host,
                    NormalizedName = UserRoles.Host.ToUpper()
                }
            };

            builder.HasData(roles);
        }
    }
}
