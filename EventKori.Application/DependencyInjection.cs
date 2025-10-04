using Microsoft.Extensions.DependencyInjection;

namespace EventKori.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // This method can be used for other application-specific services
            return services;
        }
    }
}
