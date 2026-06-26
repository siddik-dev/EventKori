using System.Text.Json;
using EventKori.Application.DTOs;
using EventKori.Domain.Entities;

namespace EventKori.Application.Mappers;

public static class EventKoriMapper
{
    public static EventRequestDto ToDto(Event eventRequest)
    {
        return new EventRequestDto
        {
            Id = eventRequest.Id,
            UserId = eventRequest.UserId,
            ClientName = GetFullName(eventRequest.User),
            Title = eventRequest.Title,
            Description = eventRequest.Description,
            EventDate = eventRequest.EventDate,
            Location = eventRequest.Location,
            AttendeesCount = eventRequest.AttendeesCount,
            Budget = eventRequest.Budget,
            EventType = eventRequest.EventType,
            Status = eventRequest.Status,
            SpecialRequirements = eventRequest.SpecialRequirements,
            QuotationCount = eventRequest.Bookings?.Count(b => b.IsActive) ?? 0
        };
    }

    public static EventRequestDetailDto ToDetailDto(Event eventRequest)
    {
        var dto = new EventRequestDetailDto
        {
            Id = eventRequest.Id,
            UserId = eventRequest.UserId,
            ClientName = GetFullName(eventRequest.User),
            Title = eventRequest.Title,
            Description = eventRequest.Description,
            EventDate = eventRequest.EventDate,
            Location = eventRequest.Location,
            AttendeesCount = eventRequest.AttendeesCount,
            Budget = eventRequest.Budget,
            EventType = eventRequest.EventType,
            Status = eventRequest.Status,
            SpecialRequirements = eventRequest.SpecialRequirements,
            QuotationCount = eventRequest.Bookings?.Count(b => b.IsActive) ?? 0,
            Quotations = eventRequest.Bookings?
                .Where(b => b.IsActive)
                .Select(ToDto)
                .ToList() ?? new List<BookingDto>()
        };

        return dto;
    }

    public static ServiceProviderDto ToDto(ServiceProvider serviceProvider)
    {
        return new ServiceProviderDto
        {
            Id = serviceProvider.Id,
            UserId = serviceProvider.UserId,
            CompanyName = serviceProvider.CompanyName,
            OwnerName = GetFullName(serviceProvider.User),
            Description = serviceProvider.Description,
            ServicesOffered = serviceProvider.ServicesOffered,
            StartingPrice = serviceProvider.StartingPrice,
            Location = serviceProvider.Location,
            Rating = serviceProvider.Rating,
            TotalReviews = serviceProvider.TotalReviews,
            YearsOfExperience = serviceProvider.YearsOfExperience,
            IsVerified = serviceProvider.IsVerified,
            CoverImageUrl = serviceProvider.PortfolioItems?.FirstOrDefault()?.ImageUrl ?? string.Empty,
            Packages = serviceProvider.Packages?
                .Where(p => p.IsActive)
                .Select(ToDto)
                .ToList() ?? new List<PackageDto>(),
            PortfolioItems = serviceProvider.PortfolioItems?
                .Where(p => p.IsActive)
                .Select(ToDto)
                .ToList() ?? new List<PortfolioItemDto>()
        };
    }

    public static ServiceProviderDetailDto ToDetailDto(ServiceProvider serviceProvider)
    {
        var dto = new ServiceProviderDetailDto
        {
            Id = serviceProvider.Id,
            UserId = serviceProvider.UserId,
            CompanyName = serviceProvider.CompanyName,
            OwnerName = GetFullName(serviceProvider.User),
            Description = serviceProvider.Description,
            ServicesOffered = serviceProvider.ServicesOffered,
            StartingPrice = serviceProvider.StartingPrice,
            Location = serviceProvider.Location,
            Rating = serviceProvider.Rating,
            TotalReviews = serviceProvider.TotalReviews,
            YearsOfExperience = serviceProvider.YearsOfExperience,
            IsVerified = serviceProvider.IsVerified,
            CoverImageUrl = serviceProvider.PortfolioItems?.FirstOrDefault()?.ImageUrl ?? string.Empty,
            Packages = serviceProvider.Packages?
                .Where(p => p.IsActive)
                .Select(ToDto)
                .ToList() ?? new List<PackageDto>(),
            PortfolioItems = serviceProvider.PortfolioItems?
                .Where(p => p.IsActive)
                .Select(ToDto)
                .ToList() ?? new List<PortfolioItemDto>(),
            Reviews = serviceProvider.Reviews?
                .Where(r => r.IsActive)
                .Select(ToDto)
                .ToList() ?? new List<ReviewDto>()
        };

        return dto;
    }

    public static PackageDto ToDto(Package package)
    {
        return new PackageDto
        {
            Id = package.Id,
            ServiceProviderId = package.ServiceProviderId,
            PackageName = package.PackageName,
            Description = package.Description,
            Price = package.Price,
            Features = ParseFeatures(package.Features),
            DurationHours = package.DurationHours,
            IsPopular = package.IsPopular,
            MaxAttendees = package.MaxAttendees
        };
    }

    public static PortfolioItemDto ToDto(PortfolioItem portfolioItem)
    {
        return new PortfolioItemDto
        {
            Id = portfolioItem.Id,
            ServiceProviderId = portfolioItem.ServiceProviderId,
            Title = portfolioItem.Title,
            Description = portfolioItem.Description,
            ImageUrl = portfolioItem.ImageUrl,
            Category = portfolioItem.Category,
            EventDate = portfolioItem.EventDate,
            Location = portfolioItem.Location,
            AttendeesCount = portfolioItem.AttendeesCount,
            Budget = portfolioItem.Budget
        };
    }

    public static BookingDto ToDto(Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            EventId = booking.EventId,
            EventTitle = booking.Event?.Title ?? string.Empty,
            ServiceProviderId = booking.ServiceProviderId,
            ProviderName = booking.ServiceProvider?.CompanyName ?? string.Empty,
            PackageId = booking.PackageId,
            PackageName = booking.Package?.PackageName ?? string.Empty,
            BookingDate = booking.BookingDate,
            EventDate = booking.EventDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            SpecialRequirements = booking.SpecialRequirements,
            PaymentStatus = booking.PaymentStatus,
            Notes = booking.Notes
        };
    }

    public static ReviewDto ToDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            ReviewerName = GetFullName(review.User),
            ReviewDate = review.ReviewDate,
            Response = review.Response
        };
    }

    private static IReadOnlyList<string> ParseFeatures(string features)
    {
        if (string.IsNullOrWhiteSpace(features))
        {
            return Array.Empty<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<IReadOnlyList<string>>(features) ?? Array.Empty<string>();
        }
        catch (JsonException)
        {
            return features.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }

    private static string GetFullName(User? user)
    {
        if (user is null)
        {
            return string.Empty;
        }

        return $"{user.FirstName} {user.LastName}".Trim();
    }
}
