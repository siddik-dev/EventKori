using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(EventKoriDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Review>> GetByServiceProviderIdAsync(int serviceProviderId)
        {
            return await _entities
                .Include(r => r.User)
                .Where(r => r.ServiceProviderId == serviceProviderId && r.IsActive)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Review>> GetByUserIdAsync(int userId)
        {
            return await _entities
                .Include(r => r.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int serviceProviderId)
        {
            var average = await _entities
                .Where(r => r.ServiceProviderId == serviceProviderId && r.IsActive)
                .AverageAsync(r => (double?)r.Rating) ?? 0.0;

            return Math.Round(average, 2);
        }

        public async Task<IReadOnlyList<Review>> GetRecentReviewsAsync(int count)
        {
            return await _entities
                .Include(r => r.User)
                .Include(r => r.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.ReviewDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Review>> GetReviewsWithUserDetailsAsync(int serviceProviderId)
        {
            return await _entities
                .Include(r => r.User)
                .Where(r => r.ServiceProviderId == serviceProviderId && r.IsActive)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review> GetReviewByBookingAsync(int bookingId)
        {
            return await _entities
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.BookingId == bookingId && r.IsActive);
        }

        public async Task<bool> HasUserReviewedBookingAsync(int bookingId, int userId)
        {
            return await _entities
                .AnyAsync(r => r.BookingId == bookingId && r.UserId == userId && r.IsActive);
        }
    }
}
