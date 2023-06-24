using Microsoft.EntityFrameworkCore;
using NovinskaAgencija.data.model;

namespace NovinskaAgencija
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Placanje>()
                .HasOne(p => p.Clanak)
                .WithOne(c => c.Placanje)
                .HasForeignKey<Placanje>(p => p.ClanakId);

            // Add any additional configuration here
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Clanak> Clanci { get; set; }
        public DbSet<ReporterClanak> ReporterClanak { get; set; }
        public DbSet<Placanje> Placanja { get; set; }
        public DbSet<Oblast> Oblasti { get; set; }
        public DbSet<ClanakOblast> ClanakOblast { get; set; }
        public DbSet<Reporter> Reporteri { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }
    }
}
