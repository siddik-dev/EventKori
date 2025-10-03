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
