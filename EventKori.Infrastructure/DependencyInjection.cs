using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using EventKori.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventKoriDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register Repositories
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
            //services.AddScoped<IPortfolioItemRepository, PortfolioItemRepository>();
            //services.AddScoped<IPricingPackageRepository, PricingPackageRepository>();
            //services.AddScoped<IEventRepository, EventRepository>();
            //services.AddScoped<IBookingRepository, BookingRepository>();
            //services.AddScoped<IReviewRepository, ReviewRepository>();

            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
