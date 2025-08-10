using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Domain
{
    public class TechnicalTestDbContext : DbContext
    {
        //Reutilizar este dbContext o crear uno a medida
        public TechnicalTestDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Movement>  Movements { get; set; }
    }
}
