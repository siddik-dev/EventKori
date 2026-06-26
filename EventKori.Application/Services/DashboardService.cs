using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using EventKori.Application.Mappers;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;

namespace EventKori.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var activeRequests = await _unitOfWork.Events.GetUpcomingEventsAsync();
        var featured = await _unitOfWork.ServiceProviders.GetFeaturedProvidersAsync(4);
        var pendingQuotes = await _unitOfWork.Bookings.GetByStatusAsync(BookingStatus.Pending);
        var confirmedQuotes = await _unitOfWork.Bookings.GetByStatusAsync(BookingStatus.Confirmed);

        return new DashboardSummaryDto
        {
            TotalEventRequests = await _unitOfWork.Events.CountAsync(),
            ActiveEventRequests = activeRequests.Count,
            VerifiedOrganizers = await _unitOfWork.ServiceProviders.CountAsync(sp => sp.IsVerified),
            PendingQuotations = pendingQuotes.Count,
            TotalQuotedValue = pendingQuotes.Concat(confirmedQuotes).Sum(q => q.TotalAmount),
            RecentRequests = activeRequests
                .OrderByDescending(e => e.CreatedAt)
                .Take(5)
                .Select(EventKoriMapper.ToDto)
                .ToList(),
            FeaturedOrganizers = featured.Select(EventKoriMapper.ToDto).ToList(),
            LatestQuotations = pendingQuotes
                .Concat(confirmedQuotes)
                .OrderByDescending(q => q.BookingDate)
                .Take(5)
                .Select(EventKoriMapper.ToDto)
                .ToList()
        };
    }
}
