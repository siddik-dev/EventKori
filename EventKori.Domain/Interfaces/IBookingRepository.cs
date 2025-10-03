using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
