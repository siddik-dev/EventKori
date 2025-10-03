using EventKori.Domain.Entities;
using EventKori.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Context
{
    public class EventKoriDbContext : IdentityDbContext<ApplicationUser>
    {
        public EventKoriDbContext(DbContextOptions<EventKoriDbContext> options) : base(options)
        {
        }

        // Domain Entities
        public DbSet<User> DomainUsers { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Domain User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.IdentityId).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            });

            // Configure Service Provider
            modelBuilder.Entity<ServiceProvider>(entity =>
            {
                entity.HasIndex(sp => sp.UserId).IsUnique();
                entity.Property(sp => sp.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(sp => sp.StartingPrice).HasPrecision(18, 2);
                entity.Property(sp => sp.Rating).HasPrecision(3, 2);

                entity.HasOne(sp => sp.User)
                    .WithOne(u => u.ServiceProvider)
                    .HasForeignKey<ServiceProvider>(sp => sp.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Portfolio Item
            modelBuilder.Entity<PortfolioItem>(entity =>
            {
                entity.Property(pi => pi.Title).IsRequired().HasMaxLength(200);
                entity.Property(pi => pi.Budget).HasPrecision(18, 2);

                entity.HasOne(pi => pi.ServiceProvider)
                    .WithMany(sp => sp.PortfolioItems)
                    .HasForeignKey(pi => pi.ServiceProviderId);
            });

            // Configure Pricing Package
            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(pp => pp.PackageName).IsRequired().HasMaxLength(200);
                entity.Property(pp => pp.Price).HasPrecision(18, 2);

                entity.HasOne(pp => pp.ServiceProvider)
                    .WithMany(sp => sp.Packages)
                    .HasForeignKey(pp => pp.ServiceProviderId);
            });

            // Configure Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Budget).HasPrecision(18, 2);
                entity.Property(e => e.EventType).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Events)
                    .HasForeignKey(e => e.UserId);
            });

            // Configure Booking
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(b => b.TotalAmount).HasPrecision(18, 2);

                entity.HasOne(b => b.Event)
                    .WithMany(e => e.Bookings)
                    .HasForeignKey(b => b.EventId);

                entity.HasOne(b => b.ServiceProvider)
                    .WithMany(sp => sp.Bookings)
                    .HasForeignKey(b => b.ServiceProviderId);

                entity.HasOne(b => b.Package)
                    .WithMany(pp => pp.Bookings)
                    .HasForeignKey(b => b.PackageId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(b => new { b.EventId, b.ServiceProviderId }).IsUnique();
            });

            // Configure Review
            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(r => r.Rating).IsRequired();
                entity.Property(r => r.Comment).HasMaxLength(1000);
                entity.Property(r => r.Response).IsRequired(false);

                entity.HasOne(r => r.Booking)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(r => r.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.ServiceProvider)
                    .WithMany(sp => sp.Reviews)
                    .HasForeignKey(r => r.ServiceProviderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(r => new { r.BookingId }).IsUnique();
            });

            // Configure enums as strings
            modelBuilder.Entity<User>()
                .Property(u => u.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Event>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();
        }
    }
}
