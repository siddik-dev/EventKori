using EventKori.Application.DTOs;

namespace EventKori.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}
