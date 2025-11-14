using Application.Contracts.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjections
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:Address"];
            options.InstanceName = configuration["Redis:AppName"];
        });
        
        services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase(configuration["DatabaseName"]!)
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
            }
        );
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFlightRepository, IFlightRepository>();
    }
}