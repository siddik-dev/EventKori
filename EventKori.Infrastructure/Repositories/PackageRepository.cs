using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EventKori.Infrastructure.Repositories
{
    public class PackageRepository : BaseRepository<Package>, IPackageRepository
    {
        public PackageRepository(EventKoriDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Package>> GetByServiceProviderIdAsync(int serviceProviderId)
        {
            return await _entities
                .Where(pp => pp.ServiceProviderId == serviceProviderId && pp.IsActive)
                .OrderBy(pp => pp.Price)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Package>> GetPopularPackagesAsync()
        {
            return await _entities
                .Include(pp => pp.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(pp => pp.IsPopular && pp.IsActive)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Package>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _entities
                .Include(pp => pp.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .Where(pp => pp.Price >= minPrice && pp.Price <= maxPrice && pp.IsActive)
                .OrderBy(pp => pp.Price)
                .ToListAsync();
        }

        public async Task<Package> GetPackageWithServiceProviderAsync(int id)
        {
            return await _entities
                .Include(pp => pp.ServiceProvider)
                .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync(pp => pp.Id == id && pp.IsActive);
        }
    }
}
