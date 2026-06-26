using EventKori.Domain.Enums;

namespace EventKori.Application.DTOs;

public class EventRequestDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int AttendeesCount { get; set; }
    public decimal Budget { get; set; }
    public string EventType { get; set; } = string.Empty;
    public EventStatus Status { get; set; }
    public string SpecialRequirements { get; set; } = string.Empty;
    public int QuotationCount { get; set; }
}

public class EventRequestDetailDto : EventRequestDto
{
    public IReadOnlyList<BookingDto> Quotations { get; set; } = Array.Empty<BookingDto>();
}

public class CreateEventRequestDto
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int AttendeesCount { get; set; }
    public decimal Budget { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string SpecialRequirements { get; set; } = string.Empty;
}

public class UpdateEventRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int AttendeesCount { get; set; }
    public decimal Budget { get; set; }
    public string EventType { get; set; } = string.Empty;
    public EventStatus Status { get; set; }
    public string SpecialRequirements { get; set; } = string.Empty;
}
