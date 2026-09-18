using Anexcs.Distro.Application.Abstractions.Persistence;
using Anexcs.Distro.Infrastructure.Persistence.Central;
using Anexcs.Distro.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anexcs.Distro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CentralDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("CentralDatabase"));
        });
        
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<CentralDbContext>());
        
        return services;
    }
}