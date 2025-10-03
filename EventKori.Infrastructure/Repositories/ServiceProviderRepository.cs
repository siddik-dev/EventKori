using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class ServiceProviderRepository : BaseRepository<ServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(EventKoriDbContext context) : base(context)
        {
        }

        public async Task<ServiceProvider> GetByUserIdAsync(int userId)
        {
            return await _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Include(sp => sp.Packages)
                .FirstOrDefaultAsync(sp => sp.UserId == userId && sp.IsActive);
        }

        public async Task<IReadOnlyList<ServiceProvider>> GetByLocationAsync(string location)
        {
            return await _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Where(sp => sp.Location.Contains(location) && sp.IsActive)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ServiceProvider>> GetByServiceTypeAsync(string serviceType)
        {
            return await _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Where(sp => sp.ServicesOffered.Contains(serviceType) && sp.IsActive)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ServiceProvider>> GetTopRatedAsync(int count)
        {
            return await _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Where(sp => sp.IsActive && sp.TotalReviews >= 5)
                .OrderByDescending(sp => sp.Rating)
                .ThenByDescending(sp => sp.TotalReviews)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ServiceProvider>> GetVerifiedProvidersAsync()
        {
            return await _entities
                .Include(sp => sp.User)
                .Where(sp => sp.IsVerified && sp.IsActive)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ServiceProvider>> SearchAsync(string searchTerm, string location = null)
        {
            var query = _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Where(sp => sp.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(sp =>
                    sp.CompanyName.Contains(searchTerm) ||
                    sp.ServicesOffered.Contains(searchTerm) ||
                    sp.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(sp => sp.Location.Contains(location));
            }

            return await query
                .OrderByDescending(sp => sp.Rating)
                .ToListAsync();
        }

        public async Task<ServiceProvider> GetWithDetailsAsync(int id)
        {
            return await _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Include(sp => sp.Packages)
                .Include(sp => sp.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(sp => sp.Id == id && sp.IsActive);
        }

        public async Task<IReadOnlyList<ServiceProvider>> GetFeaturedProvidersAsync(int count)
        {
            return await _entities
                .Include(sp => sp.User)
                .Include(sp => sp.PortfolioItems)
                .Where(sp => sp.IsVerified && sp.IsActive && sp.Rating >= 4.0m)
                .OrderByDescending(sp => sp.Rating)
                .ThenByDescending(sp => sp.TotalReviews)
                .Take(count)
                .ToListAsync();
        }

        public async Task UpdateRatingAsync(int serviceProviderId, double newRating, int totalReviews)
        {
            var provider = await GetByIdAsync(serviceProviderId);
            if (provider != null)
            {
                provider.Rating = (decimal)newRating;
                provider.TotalReviews = totalReviews;
                await UpdateAsync(provider);
            }
        }
    }
}
