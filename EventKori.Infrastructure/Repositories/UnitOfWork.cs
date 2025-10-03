using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventKoriDbContext _dbContext;
        private IDbContextTransaction _currentTransaction;

        public IUserRepository Users { get; }
        public IServiceProviderRepository ServiceProviders { get; }
        public IPortfolioItemRepository PortfolioItems { get; }
        public IPackageRepository Packages { get; }
        public IEventRepository Events { get; }
        public IBookingRepository Bookings { get; }
        public IReviewRepository Reviews { get; }

        public UnitOfWork(
            EventKoriDbContext dbContext,
            IUserRepository users,
            IServiceProviderRepository serviceProviders,
            IPortfolioItemRepository portfolioItems,
            IPackageRepository packages,
            IEventRepository events,
            IBookingRepository bookings,
            IReviewRepository reviews)
        {
            _dbContext = dbContext;
            Users = users;
            ServiceProviders = serviceProviders;
            PortfolioItems = portfolioItems;
            Packages = packages;
            Events = events;
            Bookings = bookings;
            Reviews = reviews;
        }

        public async Task<int> CompleteAsync()
        {
            // Auto-update timestamps
            foreach (var entry in _dbContext.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
