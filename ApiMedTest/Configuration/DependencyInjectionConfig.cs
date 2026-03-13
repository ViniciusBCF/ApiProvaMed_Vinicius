using ApiMedTest.Data.Context;
using ApiMedTest.Data.Repositories;
using ApiMedTest.Data.Repositories.Interfaces;
using ApiMedTest.Service.Notifications;
using ApiMedTest.Service.Notifications.Interfaces;
using ApiMedTest.Service.Service;
using ApiMedTest.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiMedTest.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection repositories)
        {
            repositories.AddScoped<IContatoRepository, ContatoRepository>();
            return repositories;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IContatoService, ContatoService>();
            services.AddScoped<INotificador, Notificador>();
            return services;
        }
    }
}
