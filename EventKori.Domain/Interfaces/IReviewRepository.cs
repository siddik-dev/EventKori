using EventKori.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
