using EventKori.Application.DTOs;
using EventKori.Domain.Enums;

namespace EventKori.Application.Interfaces;

public interface IEventRequestService
{
    Task<IReadOnlyList<EventRequestDto>> GetAllAsync(int? userId = null, string? eventType = null, EventStatus? status = null);
    Task<EventRequestDetailDto?> GetByIdAsync(int id);
    Task<EventRequestDto> CreateAsync(CreateEventRequestDto request);
    Task<EventRequestDto?> UpdateAsync(int id, UpdateEventRequestDto request);
    Task<bool> DeleteAsync(int id);
}
