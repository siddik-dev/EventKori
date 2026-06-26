namespace EventKori.Application.DTOs;

public class ServiceProviderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ServicesOffered { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public int YearsOfExperience { get; set; }
    public bool IsVerified { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public IReadOnlyList<PackageDto> Packages { get; set; } = Array.Empty<PackageDto>();
    public IReadOnlyList<PortfolioItemDto> PortfolioItems { get; set; } = Array.Empty<PortfolioItemDto>();
}

public class ServiceProviderDetailDto : ServiceProviderDto
{
    public IReadOnlyList<ReviewDto> Reviews { get; set; } = Array.Empty<ReviewDto>();
}

public class CreateServiceProviderDto
{
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ServicesOffered { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
    public string Location { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public bool IsVerified { get; set; }
    public string BusinessLicense { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}

public class UpdateServiceProviderDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ServicesOffered { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public int YearsOfExperience { get; set; }
    public bool IsVerified { get; set; }
    public string BusinessLicense { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}

public class PackageDto
{
    public int Id { get; set; }
    public int ServiceProviderId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public IReadOnlyList<string> Features { get; set; } = Array.Empty<string>();
    public int DurationHours { get; set; }
    public bool IsPopular { get; set; }
    public int MaxAttendees { get; set; }
}

public class PortfolioItemDto
{
    public int Id { get; set; }
    public int ServiceProviderId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int AttendeesCount { get; set; }
    public decimal Budget { get; set; }
}

public class ReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
    public string Response { get; set; } = string.Empty;
}
