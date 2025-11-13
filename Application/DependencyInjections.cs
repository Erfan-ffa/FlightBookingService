using Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjections
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
        });
    } 
}