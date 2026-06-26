using EventKori.Application.DTOs;

namespace EventKori.Application.Interfaces;

public interface IBookingService
{
    Task<IReadOnlyList<BookingDto>> GetAllAsync();
    Task<BookingDto?> GetByIdAsync(int id);
    Task<BookingDto> CreateAsync(CreateBookingDto request);
    Task<BookingDto?> UpdateStatusAsync(int id, UpdateBookingStatusDto request);
    Task<bool> DeleteAsync(int id);
}
