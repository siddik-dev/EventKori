using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class PortfolioItemRepository : BaseRepository<PortfolioItem>, IPortfolioItemRepository
    {
        public PortfolioItemRepository(EventKoriDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<PortfolioItem>> GetByServiceProviderIdAsync(int serviceProviderId)
        {
            return await _entities
                .Where(pi => pi.ServiceProviderId == serviceProviderId && pi.IsActive)
                .OrderByDescending(pi => pi.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PortfolioItem>> GetByCategoryAsync(string category)
        {
            return await _entities
                .Include(pi => pi.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(pi => pi.Category == category && pi.IsActive)
                .OrderByDescending(pi => pi.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PortfolioItem>> GetRecentAsync(int count)
        {
            return await _entities
                .Include(pi => pi.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(pi => pi.IsActive)
                .OrderByDescending(pi => pi.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PortfolioItem>> GetByServiceProviderAndCategoryAsync(int serviceProviderId, string category)
        {
            return await _entities
                .Where(pi => pi.ServiceProviderId == serviceProviderId &&
                            pi.Category == category &&
                            pi.IsActive)
                .OrderByDescending(pi => pi.EventDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetCategoriesByServiceProviderAsync(int serviceProviderId)
        {
            return await _entities
                .Where(pi => pi.ServiceProviderId == serviceProviderId && pi.IsActive)
                .Select(pi => pi.Category)
                .Distinct()
                .ToListAsync();
        }
    }
}
