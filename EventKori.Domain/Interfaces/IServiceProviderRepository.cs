using EventKori.Domain.Entities;

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
        Task<ServiceProvider> GetWithDetailsAsync(int id);
        Task<IReadOnlyList<ServiceProvider>> GetFeaturedProvidersAsync(int count);
        Task UpdateRatingAsync(int serviceProviderId, double newRating, int totalReviews);
    }
}
