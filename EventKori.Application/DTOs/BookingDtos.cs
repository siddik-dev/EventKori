using EventKori.Domain.Enums;

namespace EventKori.Application.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public int ServiceProviderId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public int? PackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public DateTime EventDate { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; }
    public string SpecialRequirements { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class CreateBookingDto
{
    public int EventId { get; set; }
    public int ServiceProviderId { get; set; }
    public int? PackageId { get; set; }
    public decimal TotalAmount { get; set; }
    public string SpecialRequirements { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class UpdateBookingStatusDto
{
    public BookingStatus Status { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
