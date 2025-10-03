using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IServiceProviderRepository ServiceProviders { get; }
        IPortfolioItemRepository PortfolioItems { get; }
        IPackageRepository Packages { get; }
        IEventRepository Events { get; }
        IBookingRepository Bookings { get; }
        IReviewRepository Reviews { get; }

        Task<int> CompleteAsync();
    }
}
