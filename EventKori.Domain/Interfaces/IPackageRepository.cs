using EventKori.Domain.Entities;

namespace EventKori.Domain.Interfaces
{
    public interface IPackageRepository : IBaseRepository<Package>
    {
        Task<IReadOnlyList<Package>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<Package>> GetPopularPackagesAsync();
        Task<IReadOnlyList<Package>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<Package> GetPackageWithServiceProviderAsync(int id);
    }
}
