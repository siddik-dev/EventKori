using EventKori.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Interfaces
{
    public interface IServiceProviderRepository : IBaseRepository<ServiceProvider>
    {
        Task<ServiceProvider> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<ServiceProvider>> GetByLocationAsync(string location);
        Task<IReadOnlyList<ServiceProvider>> GetByServiceTypeAsync(string serviceType);
        Task<IReadOnlyList<ServiceProvider>> GetTopRatedAsync(int count);
        Task<IReadOnlyList<ServiceProvider>> GetVerifiedProvidersAsync();
        Task<IReadOnlyList<ServiceProvider>> SearchAsync(string searchTerm, string location = null);
    }
}
