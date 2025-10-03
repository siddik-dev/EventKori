using EventKori.Domain.Entities;

namespace EventKori.Domain.Interfaces
{
    public interface IPackageRepository : IBaseRepository<Package>
    {
        Task<IReadOnlyList<Package>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<Package>> GetPopularPackagesAsync();
    }
}
