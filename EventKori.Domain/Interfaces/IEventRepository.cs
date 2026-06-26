using EventKori.Domain.Entities;
using EventKori.Domain.Enums;

namespace EventKori.Domain.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<IReadOnlyList<Event>> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<Event>> GetUpcomingEventsAsync();
        Task<IReadOnlyList<Event>> GetByStatusAsync(EventStatus status);
        Task<IReadOnlyList<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<Event>> GetEventsWithBookingsAsync(int userId);
        Task<Event> GetEventWithDetailsAsync(int id);
        Task<IReadOnlyList<Event>> GetEventsByTypeAsync(string eventType);
    }
}
