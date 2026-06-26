using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using EventKori.Application.Mappers;
using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;

namespace EventKori.Application.Services;

public class EventRequestService : IEventRequestService
{
    private readonly IUnitOfWork _unitOfWork;

    public EventRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EventRequestDto>> GetAllAsync(int? userId = null, string? eventType = null, EventStatus? status = null)
    {
        IReadOnlyList<Event> events;

        if (status.HasValue)
        {
            events = await _unitOfWork.Events.GetByStatusAsync(status.Value);
        }
        else if (!string.IsNullOrWhiteSpace(eventType))
        {
            events = await _unitOfWork.Events.GetEventsByTypeAsync(eventType);
        }
        else
        {
            events = await _unitOfWork.Events.GetUpcomingEventsAsync();
        }

        if (!string.IsNullOrWhiteSpace(eventType) && status.HasValue)
        {
            events = events
                .Where(e => string.Equals(e.EventType, eventType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (userId.HasValue)
        {
            events = events.Where(e => e.UserId == userId.Value).ToList();
        }

        return events.Select(EventKoriMapper.ToDto).ToList();
    }

    public async Task<EventRequestDetailDto?> GetByIdAsync(int id)
    {
        var eventRequest = await _unitOfWork.Events.GetEventWithDetailsAsync(id);
        return eventRequest is null ? null : EventKoriMapper.ToDetailDto(eventRequest);
    }

    public async Task<EventRequestDto> CreateAsync(CreateEventRequestDto request)
    {
        ValidateCreateRequest(request);

        var eventRequest = new Event
        {
            UserId = request.UserId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            EventDate = request.EventDate,
            Location = request.Location.Trim(),
            AttendeesCount = request.AttendeesCount,
            Budget = request.Budget,
            EventType = request.EventType.Trim(),
            SpecialRequirements = request.SpecialRequirements.Trim(),
            Status = EventStatus.Planning
        };

        await _unitOfWork.Events.AddAsync(eventRequest);
        await _unitOfWork.CompleteAsync();

        var created = await _unitOfWork.Events.GetEventWithDetailsAsync(eventRequest.Id);
        return EventKoriMapper.ToDto(created ?? eventRequest);
    }

    public async Task<EventRequestDto?> UpdateAsync(int id, UpdateEventRequestDto request)
    {
        var eventRequest = await _unitOfWork.Events.GetByIdAsync(id);
        if (eventRequest is null)
        {
            return null;
        }

        ValidateUpdateRequest(request);

        eventRequest.Title = request.Title.Trim();
        eventRequest.Description = request.Description.Trim();
        eventRequest.EventDate = request.EventDate;
        eventRequest.Location = request.Location.Trim();
        eventRequest.AttendeesCount = request.AttendeesCount;
        eventRequest.Budget = request.Budget;
        eventRequest.EventType = request.EventType.Trim();
        eventRequest.SpecialRequirements = request.SpecialRequirements.Trim();
        eventRequest.Status = request.Status;

        await _unitOfWork.Events.UpdateAsync(eventRequest);
        await _unitOfWork.CompleteAsync();

        var updated = await _unitOfWork.Events.GetEventWithDetailsAsync(id);
        return updated is null ? null : EventKoriMapper.ToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var eventRequest = await _unitOfWork.Events.GetByIdAsync(id);
        if (eventRequest is null)
        {
            return false;
        }

        await _unitOfWork.Events.DeleteAsync(eventRequest);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    private static void ValidateCreateRequest(CreateEventRequestDto request)
    {
        if (request.UserId <= 0)
        {
            throw new ArgumentException("A client user is required.");
        }

        ValidateCommon(request.Title, request.Location, request.EventType, request.AttendeesCount, request.Budget, request.EventDate);
    }

    private static void ValidateUpdateRequest(UpdateEventRequestDto request)
    {
        ValidateCommon(request.Title, request.Location, request.EventType, request.AttendeesCount, request.Budget, request.EventDate);
    }

    private static void ValidateCommon(string title, string location, string eventType, int attendeesCount, decimal budget, DateTime eventDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Event title is required.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location is required.");
        }

        if (string.IsNullOrWhiteSpace(eventType))
        {
            throw new ArgumentException("Event type is required.");
        }

        if (attendeesCount <= 0)
        {
            throw new ArgumentException("Guest count must be greater than zero.");
        }

        if (budget <= 0)
        {
            throw new ArgumentException("Budget must be greater than zero.");
        }

        if (eventDate.Date < DateTime.UtcNow.Date)
        {
            throw new ArgumentException("Event date cannot be in the past.");
        }
    }
}
