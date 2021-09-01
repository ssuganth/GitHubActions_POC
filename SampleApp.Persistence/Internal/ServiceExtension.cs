using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Persistence.Infrastructure;

namespace SampleApp.Persistence.Internal
{
    public static class ServiceExtension
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SampleAppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SampleDotnetCoreDb"),
                    b => b.MigrationsAssembly(typeof(SampleAppDbContext).Assembly.FullName)));

            services.AddScoped<SampleAppDbContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}