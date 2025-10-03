using EventKori.Domain.Entities;
using EventKori.Domain.Enums;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(EventKoriDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Event>> GetByUserIdAsync(int userId)
        {
            return await _entities
                .Include(e => e.Bookings)
                .ThenInclude(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(e => e.UserId == userId && e.IsActive)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Event>> GetUpcomingEventsAsync()
        {
            return await _entities
                .Include(e => e.User)
                .Where(e => e.EventDate >= DateTime.UtcNow && e.IsActive)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Event>> GetByStatusAsync(EventStatus status)
        {
            return await _entities
                .Include(e => e.User)
                .Where(e => e.Status == status && e.IsActive)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _entities
                .Include(e => e.User)
                .Include(e => e.Bookings)
                .Where(e => e.EventDate >= startDate && e.EventDate <= endDate && e.IsActive)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Event>> GetEventsWithBookingsAsync(int userId)
        {
            return await _entities
                .Include(e => e.Bookings)
                .ThenInclude(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Include(e => e.Bookings)
                .ThenInclude(b => b.Package)
                .Where(e => e.UserId == userId && e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<Event> GetEventWithDetailsAsync(int id)
        {
            return await _entities
                .Include(e => e.User)
                .Include(e => e.Bookings)
                .ThenInclude(b => b.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Include(e => e.Bookings)
                .ThenInclude(b => b.Package)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public async Task<IReadOnlyList<Event>> GetEventsByTypeAsync(string eventType)
        {
            return await _entities
                .Include(e => e.User)
                .Where(e => e.EventType == eventType && e.IsActive)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();
        }
    }
}
