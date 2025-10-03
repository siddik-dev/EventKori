using EventKori.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Interfaces
{
    public interface IPackageRepository : IBaseRepository<Package>
    {
        Task<IReadOnlyList<Package>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<Package>> GetPopularPackagesAsync();
    }
}
