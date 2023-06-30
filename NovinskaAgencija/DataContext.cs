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

            modelBuilder.Entity<Clanak>()
                .HasOne(c => c.Reporter)
                .WithMany(r => r.Clanak)
                .HasForeignKey(c => c.ReporterId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Oblast>().HasData(
             new Oblast { Id = 1, Name = "Politics" },
             new Oblast { Id = 2, Name = "Entertainment" },
             new Oblast { Id = 3, Name = "Business" },
             new Oblast { Id = 4, Name = "Sports" },
             new Oblast { Id = 5, Name = "Crime" },
             new Oblast { Id = 6, Name = "Education" },
             new Oblast { Id = 7, Name = "City Council News" },
             new Oblast { Id = 8, Name = "Tech" }
    );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Clanak> Clanci { get; set; }
        public DbSet<Placanje> Placanja { get; set; }
        public DbSet<Oblast> Oblasti { get; set; }
        public DbSet<Reporter> Reporteri { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }
    }
}
