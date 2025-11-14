using Application.Contracts.Repositories;
using Infrastructure.Persistence;
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
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { configuration["Redis:Address"]! },
                ConnectRetry = 3,
                ConnectTimeout = 5000,
                AsyncTimeout = 5000
            };
            options.InstanceName = configuration["Redis:AppName"];
        });
    }

    private static void ConfigureDb(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase(configuration["DatabaseName"]!)
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
            }
        );
    }
}