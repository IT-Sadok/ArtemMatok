using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;

namespace Payment.Infrastructure.DataContext
{
    public class PaymentDbContext: DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options): base(options)
        {
        }

        public DbSet<UserBalance> UserBalances { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}
