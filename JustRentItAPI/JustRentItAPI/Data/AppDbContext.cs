using Microsoft.EntityFrameworkCore;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;

namespace JustRentItAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dress> Dresses { get; set; }
        public DbSet<DressImage> DressImages { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<MonthlySummary> MonthlySummaries { get; set; }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<DressColor> DressColors { get; set; }
        public DbSet<DressSize> DressSizes { get; set; }
        public DbSet<DressAgeGroup> DressAgeGroups { get; set; }
        public DbSet<DressEventType> DressEventTypes { get; set; }
        public DbSet<DressCity> DressCities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Dress (1 לרבים)
            modelBuilder.Entity<Dress>()
                .HasOne(d => d.User)
                .WithMany(u => u.Dresses)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - DressImage (1 לרבים)
            modelBuilder.Entity<DressImage>()
                .HasOne(di => di.Dress)
                .WithMany(d => d.Images)
                .HasForeignKey(di => di.DressID)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Favorite (1 לרבים)
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - Favorite (1 לרבים)
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Dress)
                .WithMany(d => d.Favorites)
                .HasForeignKey(f => f.DressID)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Interest (1 לרבים)
            modelBuilder.Entity<Interest>()
                .HasOne(i => i.User)
                .WithMany(u => u.Interests)
                .HasForeignKey(i => i.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - Interest (1 לרבים)
            modelBuilder.Entity<Interest>()
                .HasOne(i => i.Dress)
                .WithMany(d => d.Interests)
                .HasForeignKey(i => i.DressID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - Color
            modelBuilder.Entity<DressColor>()
                .HasKey(dc => new { dc.DressID, dc.ColorID });

            modelBuilder.Entity<DressColor>()
                .HasOne(dc => dc.Dress)
                .WithMany(d => d.Colors)
                .HasForeignKey(dc => dc.DressID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DressColor>()
                .HasOne(dc => dc.Color)
                .WithMany(c => c.DressColors)
                .HasForeignKey(dc => dc.ColorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - Size
            modelBuilder.Entity<DressSize>()
                .HasKey(ds => new { ds.DressID, ds.SizeID });

            modelBuilder.Entity<DressSize>()
                .HasOne(ds => ds.Dress)
                .WithMany(d => d.Sizes)
                .HasForeignKey(ds => ds.DressID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DressSize>()
                .HasOne(ds => ds.Size)
                .WithMany(s => s.DressSizes)
                .HasForeignKey(ds => ds.SizeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - AgeGroup
            modelBuilder.Entity<DressAgeGroup>()
                .HasKey(da => new { da.DressID, da.AgeGroupID });

            modelBuilder.Entity<DressAgeGroup>()
                .HasOne(da => da.Dress)
                .WithMany(d => d.AgeGroups)
                .HasForeignKey(da => da.DressID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DressAgeGroup>()
                .HasOne(da => da.AgeGroup)
                .WithMany(a => a.DressAgeGroups)
                .HasForeignKey(da => da.AgeGroupID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - EventType
            modelBuilder.Entity<DressEventType>()
                .HasKey(de => new { de.DressID, de.EventTypeID });

            modelBuilder.Entity<DressEventType>()
                .HasOne(de => de.Dress)
                .WithMany(d => d.EventTypes)
                .HasForeignKey(de => de.DressID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DressEventType>()
                .HasOne(de => de.EventType)
                .WithMany(e => e.DressEventTypes)
                .HasForeignKey(de => de.EventTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Dress - City
            modelBuilder.Entity<DressCity>()
                .HasKey(dc => new { dc.DressID, dc.CityID });

            modelBuilder.Entity<DressCity>()
                .HasOne(dc => dc.Dress)
                .WithMany(d => d.Cities)
                .HasForeignKey(dc => dc.DressID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DressCity>()
                .HasOne(dc => dc.City)
                .WithMany(c => c.DressCities)
                .HasForeignKey(dc => dc.CityID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Interest>()
                .Property(i => i.PaymentAmount)
                .HasPrecision(10, 2);
        }
    }
}
