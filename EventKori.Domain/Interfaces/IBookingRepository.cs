using EventKori.Domain.Entities;
using EventKori.Domain.Enums;

namespace EventKori.Domain.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<IReadOnlyList<Booking>> GetByEventIdAsync(int eventId);
        Task<IReadOnlyList<Booking>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<Booking>> GetByStatusAsync(BookingStatus status);
        Task<Booking> GetByEventAndProviderAsync(int eventId, int serviceProviderId);
        Task<IReadOnlyList<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
