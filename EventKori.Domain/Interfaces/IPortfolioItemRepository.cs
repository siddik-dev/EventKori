using EventKori.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Interfaces
{
    public interface IPortfolioItemRepository : IBaseRepository<PortfolioItem>
    {
        Task<IReadOnlyList<PortfolioItem>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<PortfolioItem>> GetByCategoryAsync(string category);
        Task<IReadOnlyList<PortfolioItem>> GetRecentAsync(int count);
    }

}
