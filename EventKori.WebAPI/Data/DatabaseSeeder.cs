using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventKori.WebAPI.Data
{
    internal static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IServiceProvider provider = scope.ServiceProvider;

            EventKoriDbContext dbContext = provider.GetRequiredService<EventKoriDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            RoleManager<IdentityRole> roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Customer", "ServiceProvider" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // If data already exists, skip seeding demo data
            if (await dbContext.DomainUsers.AnyAsync())
                return;

            var users = new List<User>
            {
                new() { IdentityId = "demo-customer-1", Email = "client@eventkori.com", FirstName = "Nadia", LastName = "Rahman", PhoneNumber = "+8801712345678", Type = UserType.Customer },
                new() { IdentityId = "demo-provider-1", Email = "studio@eventkori.com", FirstName = "Rifat", LastName = "Hossain", PhoneNumber = "+8801811122233", Type = UserType.ServiceProvider },
                new() { IdentityId = "demo-provider-2", Email = "venue@eventkori.com", FirstName = "Mim", LastName = "Akter", PhoneNumber = "+8801911223344", Type = UserType.ServiceProvider },
                new() { IdentityId = "demo-provider-3", Email = "catering@eventkori.com", FirstName = "Sajid", LastName = "Karim", PhoneNumber = "+8801611223344", Type = UserType.ServiceProvider }
            };

            dbContext.DomainUsers.AddRange(users);
            await dbContext.SaveChangesAsync();

            var providers = new List<EventKori.Domain.Entities.ServiceProvider>
            {
                new()
                {
                    UserId = users[1].Id,
                    CompanyName = "Noboni Event Studio",
                    Description = "A modern event planning studio focused on elegant weddings and premium corporate experiences.",
                    ServicesOffered = "Wedding Planning, Decor, Photography",
                    StartingPrice = 250000m,
                    Location = "Dhaka",
                    Rating = 4.9m,
                    TotalReviews = 124,
                    YearsOfExperience = 8,
                    IsVerified = true,
                    BusinessLicense = "BL-2024-101",
                    TaxId = "TAX-1102"
                },
                new()
                {
                    UserId = users[2].Id,
                    CompanyName = "Meghna Venue Collective",
                    Description = "Boutique venues and decor packages for weddings, engagements, and cultural gatherings.",
                    ServicesOffered = "Venue Booking, Decor, Stage Setup",
                    StartingPrice = 180000m,
                    Location = "Gulshan",
                    Rating = 4.7m,
                    TotalReviews = 86,
                    YearsOfExperience = 6,
                    IsVerified = true,
                    BusinessLicense = "BL-2024-202",
                    TaxId = "TAX-2203"
                },
                new()
                {
                    UserId = users[3].Id,
                    CompanyName = "Sajid Catering & Events",
                    Description = "Full-service catering and event production with customizable packages for large crowds.",
                    ServicesOffered = "Catering, Lighting, Entertainment",
                    StartingPrice = 150000m,
                    Location = "Banani",
                    Rating = 4.8m,
                    TotalReviews = 98,
                    YearsOfExperience = 10,
                    IsVerified = true,
                    BusinessLicense = "BL-2024-303",
                    TaxId = "TAX-3304"
                }
            };

            dbContext.ServiceProviders.AddRange(providers);
            await dbContext.SaveChangesAsync();

            var packages = new List<Package>
            {
                new()
                {
                    ServiceProviderId = providers[0].Id,
                    PackageName = "Royal Wedding",
                    Description = "Full planning, custom decor, and premium vendor coordination.",
                    Price = 550000m,
                    Features = JsonSerializer.Serialize(new[] { "Wedding coordinator", "Bridal styling", "Premium decor", "Guest management" }),
                    DurationHours = 10,
                    IsPopular = true,
                    MaxAttendees = 500
                },
                new()
                {
                    ServiceProviderId = providers[1].Id,
                    PackageName = "Garden Celebration",
                    Description = "Venue reservation with elegant stage design and floral setup.",
                    Price = 320000m,
                    Features = JsonSerializer.Serialize(new[] { "Indoor venue", "Stage decor", "Sound system", "Lighting" }),
                    DurationHours = 8,
                    IsPopular = true,
                    MaxAttendees = 300
                },
                new()
                {
                    ServiceProviderId = providers[2].Id,
                    PackageName = "Grand Banquet",
                    Description = "Catering, entertainment, and event support for large gatherings.",
                    Price = 280000m,
                    Features = JsonSerializer.Serialize(new[] { "Buffet setup", "Live entertainment", "Cake and beverages", "Staffing" }),
                    DurationHours = 12,
                    IsPopular = false,
                    MaxAttendees = 400
                }
            };

            dbContext.Packages.AddRange(packages);

            var portfolioItems = new List<PortfolioItem>
            {
                new() { ServiceProviderId = providers[0].Id, Title = "Shahbagh Wedding Gala", Description = "Elegant winter wedding with traditional Bangladeshi touches.", ImageUrl = "https://images.unsplash.com/photo-1519225421980-715cb0215aed?auto=format&fit=crop&w=800&q=80", Category = "Wedding", EventDate = DateTime.UtcNow.AddDays(-30), Location = "Dhaka", AttendeesCount = 450, Budget = 700000m },
                new() { ServiceProviderId = providers[1].Id, Title = "Banani Corporate Mixer", Description = "A polished venue setup for a business launch event.", ImageUrl = "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=800&q=80", Category = "Corporate", EventDate = DateTime.UtcNow.AddDays(-20), Location = "Banani", AttendeesCount = 220, Budget = 450000m },
                new() { ServiceProviderId = providers[2].Id, Title = "Mehendi Night", Description = "A vibrant celebration with curated food and entertainment.", ImageUrl = "https://images.unsplash.com/photo-1522673607200-164d1b6ce486?auto=format&fit=crop&w=800&q=80", Category = "Celebration", EventDate = DateTime.UtcNow.AddDays(-15), Location = "Chattogram", AttendeesCount = 300, Budget = 380000m }
            };

            dbContext.PortfolioItems.AddRange(portfolioItems);

            var events = new List<Event>
            {
                new() { UserId = users[0].Id, Title = "Dream Wedding in Gulshan", Description = "A classic wedding celebration for 500 guests with premium planning.", EventDate = DateTime.UtcNow.AddDays(45), Location = "Dhaka", AttendeesCount = 500, Budget = 500000m, EventType = "Wedding", Status = EventStatus.Planning, SpecialRequirements = "Traditional décor, premium catering and live music." },
                new() { UserId = users[0].Id, Title = "Birthday Bash for Family", Description = "A stylish birthday celebration with a fun and colorful vibe.", EventDate = DateTime.UtcNow.AddDays(20), Location = "Chattogram", AttendeesCount = 180, Budget = 220000m, EventType = "Birthday", Status = EventStatus.Planning, SpecialRequirements = "Outdoor seating, themed décor and kids activities." },
                new() { UserId = users[0].Id, Title = "Corporate Product Launch", Description = "An elegant corporate event for clients and media.", EventDate = DateTime.UtcNow.AddDays(60), Location = "Dhaka", AttendeesCount = 250, Budget = 750000m, EventType = "Corporate", Status = EventStatus.Planning, SpecialRequirements = "Stage setup, branding, and a VIP lounge." }
            };

            dbContext.Events.AddRange(events);
            await dbContext.SaveChangesAsync();

            var bookings = new List<Booking>
            {
                new() { EventId = events[0].Id, ServiceProviderId = providers[0].Id, PackageId = packages[0].Id, EventDate = events[0].EventDate, TotalAmount = 575000m, Status = BookingStatus.Pending, SpecialRequirements = "Need full planning and vendor coordination.", PaymentStatus = "Pending", Notes = "Quotation sent for the wedding package." },
                new() { EventId = events[1].Id, ServiceProviderId = providers[1].Id, PackageId = packages[1].Id, EventDate = events[1].EventDate, TotalAmount = 310000m, Status = BookingStatus.Confirmed, SpecialRequirements = "Venue should be close to the family home.", PaymentStatus = "Paid", Notes = "Venue booking confirmed." },
                new() { EventId = events[2].Id, ServiceProviderId = providers[2].Id, PackageId = packages[2].Id, EventDate = events[2].EventDate, TotalAmount = 320000m, Status = BookingStatus.Pending, SpecialRequirements = "Need branded buffet and entertainment.", PaymentStatus = "Pending", Notes = "Quote is in review." }
            };

            dbContext.Bookings.AddRange(bookings);
            await dbContext.SaveChangesAsync();

            var reviews = new List<Review>
            {
                new() { BookingId = bookings[0].Id, ServiceProviderId = providers[0].Id, UserId = users[0].Id, Rating = 5, Comment = "Everything was beautifully coordinated and on time.", ReviewDate = DateTime.UtcNow.AddDays(-3), Response = "Thank you for the trust!" },
                new() { BookingId = bookings[1].Id, ServiceProviderId = providers[1].Id, UserId = users[0].Id, Rating = 4, Comment = "Great venue and helpful staff.", ReviewDate = DateTime.UtcNow.AddDays(-2), Response = "We are delighted to support your event." }
            };

            dbContext.Reviews.AddRange(reviews);
            await dbContext.SaveChangesAsync();
        }
    }
}