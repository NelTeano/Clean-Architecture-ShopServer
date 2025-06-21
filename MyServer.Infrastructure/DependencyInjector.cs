using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyServer.Infrastructure.Data;
using MyServer.Infrastructure.Repositories;
using MyServer.Core.Interfaces;

namespace MyServer.Infrastructure
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Register the DbContext
            services.AddDbContext<ApplicationContextDB>(options =>
                options.UseSqlServer(connectionString));

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
