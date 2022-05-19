using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.DBContext
{

    public class ParagonIntegrationDBContext : DbContext
    {
        public ParagonIntegrationDBContext(DbContextOptions<ParagonIntegrationDBContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ParagonJWT> ParagonJWT { get; set; }
    }
}
