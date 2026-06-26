namespace EventKori.Application.DTOs;

public class DashboardSummaryDto
{
    public int TotalEventRequests { get; set; }
    public int ActiveEventRequests { get; set; }
    public int VerifiedOrganizers { get; set; }
    public int PendingQuotations { get; set; }
    public decimal TotalQuotedValue { get; set; }
    public IReadOnlyList<EventRequestDto> RecentRequests { get; set; } = Array.Empty<EventRequestDto>();
    public IReadOnlyList<ServiceProviderDto> FeaturedOrganizers { get; set; } = Array.Empty<ServiceProviderDto>();
    public IReadOnlyList<BookingDto> LatestQuotations { get; set; } = Array.Empty<BookingDto>();
}
