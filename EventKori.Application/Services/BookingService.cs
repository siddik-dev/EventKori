using EventKori.Application.DTOs;
using EventKori.Application.Interfaces;
using EventKori.Application.Mappers;
using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;

namespace EventKori.Application.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<BookingDto>> GetAllAsync()
    {
        var bookings = await _unitOfWork.Bookings.GetByStatusAsync(BookingStatus.Pending);
        var confirmed = await _unitOfWork.Bookings.GetByStatusAsync(BookingStatus.Confirmed);

        return bookings
            .Concat(confirmed)
            .OrderByDescending(b => b.BookingDate)
            .Select(EventKoriMapper.ToDto)
            .ToList();
    }

    public async Task<BookingDto?> GetByIdAsync(int id)
    {
        var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(id);
        return booking is null ? null : EventKoriMapper.ToDto(booking);
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto request)
    {
        ValidateCreateRequest(request);

        var eventRequest = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (eventRequest is null)
        {
            throw new ArgumentException("Event request was not found.");
        }

        if (!await _unitOfWork.ServiceProviders.ExistsAsync(request.ServiceProviderId))
        {
            throw new ArgumentException("Organizer was not found.");
        }

        if (request.PackageId.HasValue && !await _unitOfWork.Packages.ExistsAsync(request.PackageId.Value))
        {
            throw new ArgumentException("Package was not found.");
        }

        var existing = await _unitOfWork.Bookings.GetByEventAndProviderAsync(request.EventId, request.ServiceProviderId);
        if (existing is not null)
        {
            throw new ArgumentException("This organizer has already sent a quotation for the event.");
        }

        var booking = new Booking
        {
            EventId = request.EventId,
            ServiceProviderId = request.ServiceProviderId,
            PackageId = request.PackageId,
            EventDate = eventRequest.EventDate,
            TotalAmount = request.TotalAmount,
            Status = BookingStatus.Pending,
            PaymentStatus = "Pending",
            SpecialRequirements = request.SpecialRequirements.Trim(),
            Notes = request.Notes.Trim()
        };

        await _unitOfWork.Bookings.AddAsync(booking);
        await _unitOfWork.CompleteAsync();

        var created = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(booking.Id);
        return EventKoriMapper.ToDto(created ?? booking);
    }

    public async Task<BookingDto?> UpdateStatusAsync(int id, UpdateBookingStatusDto request)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        if (booking is null)
        {
            return null;
        }

        booking.Status = request.Status;
        booking.PaymentStatus = string.IsNullOrWhiteSpace(request.PaymentStatus)
            ? booking.PaymentStatus
            : request.PaymentStatus.Trim();
        booking.Notes = string.IsNullOrWhiteSpace(request.Notes)
            ? booking.Notes
            : request.Notes.Trim();

        if (request.Status == BookingStatus.Confirmed)
        {
            var eventRequest = await _unitOfWork.Events.GetByIdAsync(booking.EventId);
            if (eventRequest is not null)
            {
                eventRequest.Status = EventStatus.Booked;
                await _unitOfWork.Events.UpdateAsync(eventRequest);
            }
        }

        await _unitOfWork.Bookings.UpdateAsync(booking);
        await _unitOfWork.CompleteAsync();

        var updated = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(id);
        return updated is null ? null : EventKoriMapper.ToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        if (booking is null)
        {
            return false;
        }

        await _unitOfWork.Bookings.DeleteAsync(booking);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    private static void ValidateCreateRequest(CreateBookingDto request)
    {
        if (request.EventId <= 0)
        {
            throw new ArgumentException("Event request is required.");
        }

        if (request.ServiceProviderId <= 0)
        {
            throw new ArgumentException("Organizer is required.");
        }

        if (request.TotalAmount <= 0)
        {
            throw new ArgumentException("Quotation amount must be greater than zero.");
        }
    }
}
