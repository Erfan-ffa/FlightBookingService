using Application.Contracts;
using Application.Utils;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjections
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(IAssemblyMarker).Assembly);
   
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
            configure.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
    } 
}