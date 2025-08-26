using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using MyServer.Application.Mappings;
using System.Reflection;


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

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<VariantProfile>();
            });

            return services;
        }
    }
}
