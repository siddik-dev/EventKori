using EventKori.Domain.Entities;

namespace EventKori.Domain.Interfaces
{
    public interface IPortfolioItemRepository : IBaseRepository<PortfolioItem>
    {
        Task<IReadOnlyList<PortfolioItem>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<PortfolioItem>> GetByCategoryAsync(string category);
        Task<IReadOnlyList<PortfolioItem>> GetRecentAsync(int count);
    }

}
