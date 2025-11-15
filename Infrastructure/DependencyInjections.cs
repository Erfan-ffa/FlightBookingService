using Application.Contracts;
using Application.Contracts.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.cache;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjections
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureRedis(services, configuration);
        ConfigureDb(services, configuration);
        
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration["Redis:Address"]!);
        var redLockFactory = RedLockFactory.Create([connectionMultiplexer]);
        services.AddSingleton<IDistributedLockFactory>(redLockFactory);

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFlightRepository, FlightRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
    }

    private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(x =>
            ConnectionMultiplexer.Connect(configuration["Redis:Address"]!)
        );
        
        services.AddScoped<IDatabase>(sp =>
            sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase()
        );
        
        services.AddScoped<ICacheService, CacheService>();
    }

    private static void ConfigureDb(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
            }
        );
    }
}