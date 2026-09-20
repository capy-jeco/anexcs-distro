using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.FluentValidation;

namespace Anexcs.Distro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddWolverine(opts =>
        {
            opts.Discovery.IncludeAssembly(
                typeof(DependencyInjection).Assembly);

            opts.UseFluentValidation(
                RegistrationBehavior.ExplicitRegistration);
        });
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}