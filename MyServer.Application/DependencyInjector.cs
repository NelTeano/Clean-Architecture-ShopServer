using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;


namespace MyServer.Application
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjector).Assembly);
            });
            return services;
        }
    }
}
