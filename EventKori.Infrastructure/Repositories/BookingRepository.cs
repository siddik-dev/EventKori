using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(EventKoriDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Booking>> GetByEventIdAsync(int eventId)
        {
            return await _entities
                .Include(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Include(b => b.Package)
                .Where(b => b.EventId == eventId && b.IsActive)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Booking>> GetByServiceProviderIdAsync(int serviceProviderId)
        {
            return await _entities
                .Include(b => b.Event)
                .ThenInclude(e => e.User)
                .Include(b => b.Package)
                .Where(b => b.ServiceProviderId == serviceProviderId && b.IsActive)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Booking>> GetByStatusAsync(BookingStatus status)
        {
            return await _entities
                .Include(b => b.Event)
                .Include(b => b.ServiceProvider)
                .Where(b => b.Status == status && b.IsActive)
                .ToListAsync();
        }

        public async Task<Booking> GetByEventAndProviderAsync(int eventId, int serviceProviderId)
        {
            return await _entities
                .FirstOrDefaultAsync(b => b.EventId == eventId &&
                                        b.ServiceProviderId == serviceProviderId &&
                                        b.IsActive);
        }

        public async Task<IReadOnlyList<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _entities
                .Include(b => b.Event)
                .Include(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(b => b.EventDate >= startDate && b.EventDate <= endDate && b.IsActive)
                .OrderBy(b => b.EventDate)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingWithDetailsAsync(int id)
        {
            return await _entities
                .Include(b => b.Event)
                .ThenInclude(e => e.User)
                .Include(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Include(b => b.Package)
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
        }

        public async Task<IReadOnlyList<Booking>> GetUpcomingBookingsAsync(int userId)
        {
            return await _entities
                .Include(b => b.Event)
                .Include(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(b => b.Event.UserId == userId &&
                           b.EventDate >= DateTime.UtcNow &&
                           b.Status != BookingStatus.Cancelled &&
                           b.Status != BookingStatus.Completed &&
                           b.IsActive)
                .OrderBy(b => b.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Booking>> GetServiceProviderBookingsByStatusAsync(int serviceProviderId, BookingStatus status)
        {
            return await _entities
                .Include(b => b.Event)
                .ThenInclude(e => e.User)
                .Where(b => b.ServiceProviderId == serviceProviderId &&
                           b.Status == status &&
                           b.IsActive)
                .OrderBy(b => b.EventDate)
                .ToListAsync();
        }
    }
}
