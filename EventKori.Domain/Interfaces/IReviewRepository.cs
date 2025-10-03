using EventKori.Domain.Entities;

namespace EventKori.Domain.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IReadOnlyList<Review>> GetByServiceProviderIdAsync(int serviceProviderId);
        Task<IReadOnlyList<Review>> GetByUserIdAsync(int userId);
        Task<double> GetAverageRatingAsync(int serviceProviderId);
        Task<IReadOnlyList<Review>> GetRecentReviewsAsync(int count);
    }

}
