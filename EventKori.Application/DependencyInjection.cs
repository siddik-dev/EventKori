using EventKori.Application.Interfaces;
using EventKori.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EventKori.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IEventRequestService, EventRequestService>();
            services.AddScoped<IServiceProviderService, ServiceProviderService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddValidatorsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
